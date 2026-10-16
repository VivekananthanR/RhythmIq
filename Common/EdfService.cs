using System.Globalization;
using System.Text;
public interface IEdfService
{
    string MergeEdfFiles(List<string> sourceFiles, DateTime fromTime, DateTime toTime, string fullOutputPath);
}

public class EdfService : IEdfService
{
    private class EdfSignalInfo
    {
        public string Label { get; set; }
        public byte[] RawHeaderBlock { get; set; } // Stores all 256 bytes of signal metadata
        public int SamplesPerRecord { get; set; }
        public double SamplingFrequency { get; set; }
        public short DigitalZero { get; set; }
    }

    private class EdfFileMetadata
    {
        public string FilePath { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int HeaderBytes { get; set; }
        public int NumDataRecords { get; set; }
        public double RecordDurationSeconds { get; set; }
        public int NumSignals { get; set; }
        public byte[] MainHeaderBytes { get; set; }
        public List<EdfSignalInfo> Signals { get; set; } = new List<EdfSignalInfo>();
    }

    public string MergeEdfFiles(
        List<string> sourceFiles,
        DateTime fromTime,
        DateTime toTime,
        string fullOutputPath)
    {
        if (sourceFiles == null || sourceFiles.Count == 0)
            return string.Empty;

        if (toTime <= fromTime)
            return string.Empty;

        // 1. Parse metadata from all valid EDF files
        var validFiles = new List<EdfFileMetadata>();
        foreach (var file in sourceFiles)
        {
            if (!File.Exists(file)) continue;
            var meta = ReadEdfMetadata(file);
            if (meta != null && meta.EndTime > fromTime && meta.StartTime < toTime)
            {
                validFiles.Add(meta);
            }
        }

        if (validFiles.Count == 0)
            return string.Empty;

        // Sort chronologically by start time
        validFiles = validFiles.OrderBy(f => f.StartTime).ToList();

        // Use the first file as the structural template (channels, sample rates, physical/digital scaling)
        var template = validFiles.First();
        int numSignals = template.NumSignals;

        // Calculate exact duration in whole seconds for standard 1-second EDF data records
        int totalSeconds = (int)Math.Ceiling((toTime - fromTime).TotalSeconds);
        if (totalSeconds <= 0)
            return string.Empty;

        // 2. Allocate output sample buffers for each channel across the requested time window
        short[][] mergedChannelSamples = new short[numSignals][];
        for (int s = 0; s < numSignals; s++)
        {
            int sampleRate = (int)Math.Round(template.Signals[s].SamplingFrequency);
            int totalSamples = totalSeconds * sampleRate;
            mergedChannelSamples[s] = new short[totalSamples];

            // Pre-fill with digital zero (flatline) in case there are small time gaps between files
            if (template.Signals[s].DigitalZero != 0)
            {
                Array.Fill(mergedChannelSamples[s], template.Signals[s].DigitalZero);
            }
        }

        // 3. Read sample data from each overlapping file and map into the output timeline
        foreach (var fileMeta in validFiles)
        {
            // Skip if channel count doesn't match the template
            if (fileMeta.NumSignals != numSignals)
                continue;

            ReadAndMapSamples(fileMeta, fromTime, toTime, mergedChannelSamples);
        }

        // 4. Write the newly merged and trimmed EDF file to disk
        WriteMergedEdf(fullOutputPath, template, fromTime, totalSeconds, mergedChannelSamples);

        return File.Exists(fullOutputPath) ? fullOutputPath : string.Empty;
    }

    private EdfFileMetadata ReadEdfMetadata(string filePath)
    {
        using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        if (fs.Length < 256) return null;

        byte[] mainHeader = new byte[256];
        fs.Read(mainHeader, 0, 256);

        string startDateStr = Encoding.ASCII.GetString(mainHeader, 168, 8).Trim(); // dd.mm.yy
        string startTimeStr = Encoding.ASCII.GetString(mainHeader, 176, 8).Trim(); // hh.mm.ss
        string headerBytesStr = Encoding.ASCII.GetString(mainHeader, 184, 8).Trim();
        string numRecordsStr = Encoding.ASCII.GetString(mainHeader, 236, 8).Trim();
        string durationStr = Encoding.ASCII.GetString(mainHeader, 244, 8).Trim();
        string numSignalsStr = Encoding.ASCII.GetString(mainHeader, 252, 4).Trim();

        if (!int.TryParse(headerBytesStr, out int headerBytes) ||
            !int.TryParse(numRecordsStr, out int numRecords) ||
            !double.TryParse(durationStr, NumberStyles.Any, CultureInfo.InvariantCulture, out double recordDuration) ||
            !int.TryParse(numSignalsStr, out int numSignals) ||
            numSignals <= 0 || recordDuration <= 0)
        {
            return null;
        }

        DateTime startTime = ParseEdfDateTime(startDateStr, startTimeStr);

        // Read Signal Headers (numSignals * 256 bytes)
        int signalHeaderLength = numSignals * 256;
        if (fs.Length < 256 + signalHeaderLength) return null;

        byte[] signalHeaders = new byte[signalHeaderLength];
        fs.Read(signalHeaders, 0, signalHeaderLength);

        var meta = new EdfFileMetadata
        {
            FilePath = filePath,
            StartTime = startTime,
            HeaderBytes = headerBytes,
            NumDataRecords = numRecords,
            RecordDurationSeconds = recordDuration,
            NumSignals = numSignals,
            MainHeaderBytes = mainHeader
        };

        int bytesPerRecord = 0;

        for (int s = 0; s < numSignals; s++)
        {
            string label = ReadSignalField(signalHeaders, numSignals, s, 0, 16);
            string physMinStr = ReadSignalField(signalHeaders, numSignals, s, 104, 8);
            string physMaxStr = ReadSignalField(signalHeaders, numSignals, s, 112, 8);
            string digMinStr = ReadSignalField(signalHeaders, numSignals, s, 120, 8);
            string digMaxStr = ReadSignalField(signalHeaders, numSignals, s, 128, 8);
            string samplesStr = ReadSignalField(signalHeaders, numSignals, s, 216, 8);

            int.TryParse(samplesStr, out int samplesPerRec);
            bytesPerRecord += samplesPerRec * 2;

            // Calculate digital value corresponding to physical 0 (baseline flatline)
            short digitalZero = 0;
            if (double.TryParse(physMinStr, NumberStyles.Any, CultureInfo.InvariantCulture, out double physMin) &&
                double.TryParse(physMaxStr, NumberStyles.Any, CultureInfo.InvariantCulture, out double physMax) &&
                double.TryParse(digMinStr, NumberStyles.Any, CultureInfo.InvariantCulture, out double digMin) &&
                double.TryParse(digMaxStr, NumberStyles.Any, CultureInfo.InvariantCulture, out double digMax) &&
                Math.Abs(physMax - physMin) > 1e-9)
            {
                double zeroVal = digMin + (0.0 - physMin) * (digMax - digMin) / (physMax - physMin);
                digitalZero = (short)Math.Clamp(Math.Round(zeroVal), short.MinValue, short.MaxValue);
            }

            meta.Signals.Add(new EdfSignalInfo
            {
                Label = label,
                SamplesPerRecord = samplesPerRec,
                SamplingFrequency = samplesPerRec / recordDuration,
                DigitalZero = digitalZero
            });
        }

        // If header has -1 for numRecords (unfinished stream), infer from actual file size
        if (meta.NumDataRecords <= 0 && bytesPerRecord > 0)
        {
            long dataBytes = fs.Length - headerBytes;
            meta.NumDataRecords = (int)(dataBytes / bytesPerRecord);
        }

        meta.EndTime = meta.StartTime.AddSeconds(meta.NumDataRecords * meta.RecordDurationSeconds);

        // Save raw signal header block on the first signal for easy rewriting
        meta.Signals[0].RawHeaderBlock = signalHeaders;

        return meta;
    }

    private void ReadAndMapSamples(
        EdfFileMetadata fileMeta,
        DateTime targetFrom,
        DateTime targetTo,
        short[][] outputBuffers)
    {
        using var fs = new FileStream(fileMeta.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using var reader = new BinaryReader(fs);

        int totalSamplesPerRecord = fileMeta.Signals.Sum(s => s.SamplesPerRecord);
        int bytesPerRecord = totalSamplesPerRecord * 2;

        for (int rec = 0; rec < fileMeta.NumDataRecords; rec++)
        {
            DateTime recStart = fileMeta.StartTime.AddSeconds(rec * fileMeta.RecordDurationSeconds);
            DateTime recEnd = recStart.AddSeconds(fileMeta.RecordDurationSeconds);

            // Skip records outside our target window
            if (recEnd <= targetFrom || recStart >= targetTo)
                continue;

            long recordFileOffset = fileMeta.HeaderBytes + ((long)rec * bytesPerRecord);
            if (recordFileOffset + bytesPerRecord > fs.Length)
                break;

            fs.Seek(recordFileOffset, SeekOrigin.Begin);

            for (int s = 0; s < fileMeta.NumSignals; s++)
            {
                var sig = fileMeta.Signals[s];
                double sampleRate = sig.SamplingFrequency;

                byte[] rawBytes = reader.ReadBytes(sig.SamplesPerRecord * 2);
                short[] recordSamples = new short[sig.SamplesPerRecord];
                Buffer.BlockCopy(rawBytes, 0, recordSamples, 0, rawBytes.Length);

                for (int i = 0; i < sig.SamplesPerRecord; i++)
                {
                    DateTime sampleTime = recStart.AddSeconds(i / sampleRate);
                    if (sampleTime < targetFrom || sampleTime >= targetTo)
                        continue;

                    int destIndex = (int)Math.Floor((sampleTime - targetFrom).TotalSeconds * sampleRate);
                    if (destIndex >= 0 && destIndex < outputBuffers[s].Length)
                    {
                        outputBuffers[s][destIndex] = recordSamples[i];
                    }
                }
            }
        }
    }

    private void WriteMergedEdf(
        string outputPath,
        EdfFileMetadata template,
        DateTime startTime,
        int numRecordsOneSec,
        short[][] channelSamples)
    {
        int numSignals = template.NumSignals;
        byte[] mainHeader = (byte[])template.MainHeaderBytes.Clone();
        byte[] signalHeaders = (byte[])template.Signals[0].RawHeaderBlock.Clone();

        // Update Main Header fields: StartDate, StartTime, NumDataRecords, RecordDuration (1 sec)
        WriteAsciiField(mainHeader, 168, 8, startTime.ToString("dd.MM.yy", CultureInfo.InvariantCulture));
        WriteAsciiField(mainHeader, 176, 8, startTime.ToString("HH.mm.ss", CultureInfo.InvariantCulture));
        WriteAsciiField(mainHeader, 236, 8, numRecordsOneSec.ToString(CultureInfo.InvariantCulture));
        WriteAsciiField(mainHeader, 244, 8, "1"); // Normalize output to 1-second data records

        // Update Signal Header SamplesPerRecord to match 1-second duration
        int[] samplesPerOneSecRecord = new int[numSignals];
        for (int s = 0; s < numSignals; s++)
        {
            samplesPerOneSecRecord[s] = (int)Math.Round(template.Signals[s].SamplingFrequency);
            WriteSignalField(signalHeaders, numSignals, s, 216, 8, samplesPerOneSecRecord[s].ToString(CultureInfo.InvariantCulture));
        }

        using var fs = new FileStream(outputPath, FileMode.Create, FileAccess.Write, FileShare.None);
        using var writer = new BinaryWriter(fs);

        // 1. Write 256-byte Main Header + (N * 256)-byte Signal Headers
        writer.Write(mainHeader);
        writer.Write(signalHeaders);

        // 2. Write interleaved Data Records (1 second per record)
        for (int rec = 0; rec < numRecordsOneSec; rec++)
        {
            for (int s = 0; s < numSignals; s++)
            {
                int count = samplesPerOneSecRecord[s];
                int startIdx = rec * count;

                byte[] byteBuffer = new byte[count * 2];
                Buffer.BlockCopy(channelSamples[s], startIdx * 2, byteBuffer, 0, byteBuffer.Length);
                writer.Write(byteBuffer);
            }
        }
    }

    private DateTime ParseEdfDateTime(string dateStr, string timeStr)
    {
        // EDF standard uses dd.MM.yy and HH.mm.ss
        string[] dParts = dateStr.Split('.');
        string[] tParts = timeStr.Split('.', ':');

        int day = int.Parse(dParts[0]);
        int month = int.Parse(dParts[1]);
        int yearTwoDigit = int.Parse(dParts[2]);
        // EDF clipping rule: 85-99 -> 1985-1999, 00-84 -> 2000-2084
        int year = yearTwoDigit >= 85 ? 1900 + yearTwoDigit : 2000 + yearTwoDigit;

        int hour = int.Parse(tParts[0]);
        int minute = int.Parse(tParts[1]);
        int second = int.Parse(tParts[2]);

        return new DateTime(year, month, day, hour, minute, second);
    }

    private string ReadSignalField(byte[] signalHeaderBlock, int numSignals, int sigIndex, int fieldOffsetPerSig, int fieldLength)
    {
        int offset = (fieldOffsetPerSig * numSignals) + (sigIndex * fieldLength);
        return Encoding.ASCII.GetString(signalHeaderBlock, offset, fieldLength).Trim();
    }

    private void WriteSignalField(byte[] signalHeaderBlock, int numSignals, int sigIndex, int fieldOffsetPerSig, int fieldLength, string value)
    {
        int offset = (fieldOffsetPerSig * numSignals) + (sigIndex * fieldLength);
        WriteAsciiField(signalHeaderBlock, offset, fieldLength, value);
    }

    private void WriteAsciiField(byte[] buffer, int offset, int length, string value)
    {
        string padded = (value ?? string.Empty).PadRight(length, ' ');
        if (padded.Length > length) padded = padded.Substring(0, length);
        byte[] bytes = Encoding.ASCII.GetBytes(padded);
        Buffer.BlockCopy(bytes, 0, buffer, offset, length);
    }
}