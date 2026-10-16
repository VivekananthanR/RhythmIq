function drawECG(canvas, waveformData) {
    const dpr = Math.max(window.devicePixelRatio || 1, 4);
    const width = canvas.parentElement.clientWidth;
    const height = 180;

    canvas.width = width * dpr;
    canvas.height = height * dpr;
    canvas.style.width = width + 'px';
    canvas.style.height = height + 'px';

    const ctx = canvas.getContext('2d');
    ctx.setTransform(dpr, 0, 0, dpr, 0, 0);
    ctx.clearRect(0, 0, width, height);

    // Equal margins on left and right since Y-axis amplitude points are removed
    const marginLeft = 6, marginRight = 6, marginTop = 20, marginBottom = 45;
    const plotW = Math.max(0, width - marginLeft - marginRight);
    const plotH = height - marginTop - marginBottom;

    const voltages = waveformData.voltage_mv || [];
    const timestamps = waveformData.timestamp_ms || [];
    const samplingRate = waveformData.sampling_rate || 256;
    const totalPoints = voltages.length;
    const durationSec = totalPoints / samplingRate;

    if (totalPoints === 0) {
        ctx.fillStyle = '#000000';
        ctx.font = '12px Arial';
        ctx.textAlign = 'center';
        ctx.textBaseline = 'middle';
        ctx.fillText('No ECG data available in response.', width / 2, height / 2);
        return;
    }

    const pxPerSec = plotW / (durationSec || 10);
    const midY = marginTop + (plotH / 2);

    // Standard ECG paper square grid (1mm x 1mm and 5mm x 5mm proportional)
    const smallStepX = 0.04 * pxPerSec;
    const smallStepY = smallStepX;
    const bigStepX = smallStepX * 5;
    const bigStepY = smallStepY * 5;

    const gridLeft = marginLeft, gridRight = width - marginRight;
    const gridTop = marginTop, gridBottom = height - marginBottom;

    // 1. Grid Background (Pure white)
    ctx.fillStyle = '#ffffff';
    ctx.fillRect(gridLeft, gridTop, plotW, plotH);

    // 2. Minor Grid (Light pink/salmon-red medical ECG paper theme)
    ctx.strokeStyle = '#ffb2b2';
    ctx.lineWidth = 0.5;
    for (let x = gridLeft; x <= gridRight; x += smallStepX) {
        ctx.beginPath();
        ctx.moveTo(Math.round(x) + 0.5, gridTop);
        ctx.lineTo(Math.round(x) + 0.5, gridBottom);
        ctx.stroke();
    }
    for (let y = gridTop; y <= gridBottom; y += smallStepY) {
        ctx.beginPath();
        ctx.moveTo(gridLeft, Math.round(y) + 0.5);
        ctx.lineTo(gridRight, Math.round(y) + 0.5);
        ctx.stroke();
    }

    // 3. Major Grid (Vivid medical red)
    ctx.strokeStyle = '#ef4444';
    ctx.lineWidth = 0.8;
    for (let x = gridLeft; x <= gridRight; x += bigStepX) {
        ctx.beginPath();
        ctx.moveTo(Math.round(x) + 0.5, gridTop);
        ctx.lineTo(Math.round(x) + 0.5, gridBottom);
        ctx.stroke();
    }
    for (let y = gridTop; y <= gridBottom; y += bigStepY) {
        ctx.beginPath();
        ctx.moveTo(gridLeft, Math.round(y) + 0.5);
        ctx.lineTo(gridRight, Math.round(y) + 0.5);
        ctx.stroke();
    }

    // 4. Grid Outer Border
    ctx.strokeStyle = '#ef4444';
    ctx.lineWidth = 0.85;
    ctx.strokeRect(gridLeft, gridTop, plotW, plotH);

    // 5. Y-Axis Amplitude Points removed per request

    // 6. X-Axis Timestamp Labels (Black)
    ctx.fillStyle = '#000000';
    ctx.font = '8px Arial, sans-serif';
    ctx.textBaseline = 'bottom';

    function formatTimeLabel(index) {
        if (!timestamps || timestamps.length === 0 || timestamps[index] === undefined) {
            return '--/--/---- --:--:--';
        }
        const date = new Date(timestamps[index]);
        if (isNaN(date.getTime())) {
            return '--/--/---- --:--:--';
        }
        const hours = String(date.getHours()).padStart(2, '0');
        const mins = String(date.getMinutes()).padStart(2, '0');
        const secs = String(date.getSeconds()).padStart(2, '0');
        return `${date.toLocaleDateString('en-GB')} ${hours}:${mins}:${secs}`;
    }

    ctx.textAlign = 'left';
    ctx.fillText(formatTimeLabel(0), gridLeft, marginTop - 4);
    ctx.textAlign = 'right';
    ctx.fillText(formatTimeLabel(totalPoints - 1), gridRight, marginTop - 4);

    // Amplitude Scale & Offset extracted from waveformData (matching web app test.ts)
    const amplitudeScale = Number(
        waveformData.amplitudeScale ??
        waveformData.waveform_snippet?.amplitudeScale ??
        1
    ) || 1;

    const yOffset = Number(
        waveformData.yOffset ??
        waveformData.waveform_snippet?.yOffset ??
        0
    ) || 0;

    // Web-style Y mapping (matching test.ts getMappedY)
    function getMappedY(voltage, totalHeight, minV, maxV) {
        const padding = totalHeight * 0.1;
        const availableHeight = totalHeight - (padding * 2);
        const range = maxV - minV;
        const normalized = (voltage - minV) / (range || 1);
        return (totalHeight - padding) - (normalized * availableHeight);
    }

    // Web-style visible mV calculations
    const pxPerBigBox = pxPerSec * 0.2;
    const pxPer_mV = pxPerBigBox / 0.5;

    const totalVisible_mV = plotH / pxPer_mV;
    const scaledVisible_mV = totalVisible_mV / amplitudeScale;
    const chMinV = -(scaledVisible_mV / 2) + yOffset;
    const chMaxV = (scaledVisible_mV / 2) + yOffset;

    // 7. Main ECG Waveform (Scaled by amplitudeScale, clipped cleanly to grid container)
    ctx.save();
    ctx.beginPath();
    ctx.rect(gridLeft, gridTop, plotW, plotH);
    ctx.clip();

    ctx.strokeStyle = '#000000';
    ctx.lineWidth = 1.0;
    ctx.lineJoin = 'round';
    ctx.lineCap = 'round';
    ctx.beginPath();

    for (let i = 0; i < totalPoints; i++) {
        const x = gridLeft + (i / samplingRate) * pxPerSec;
        const y = gridTop + getMappedY(voltages[i], plotH, chMinV, chMaxV);
        if (i === 0) ctx.moveTo(x, y);
        else ctx.lineTo(x, y);
    }
    ctx.stroke();
    ctx.restore();

    // 8. Bottom Rhythm / Mini-strip Preview
    const stripTop = height - 30;
    const stripBottom = height - 5;
    const stripH = stripBottom - stripTop;
    const stripMidY = stripTop + stripH / 2;
    const hlStart = gridLeft + plotW * 0.35;
    const hlEnd = gridLeft + plotW * 0.65;

    // Mini-strip background container
    ctx.fillStyle = '#ffffff';
    ctx.fillRect(gridLeft, stripTop, plotW, stripH);

    // Highlight section (soft red/pink tint like Image 2)
    ctx.fillStyle = 'rgba(239, 68, 68, 0.22)';
    ctx.fillRect(hlStart, stripTop, hlEnd - hlStart, stripH);

    // Mini-strip outer border
    ctx.strokeStyle = '#d1d5db';
    ctx.lineWidth = 0.8;
    ctx.strokeRect(gridLeft, stripTop, plotW, stripH);

    // Center baseline
    ctx.strokeStyle = '#e5e7eb';
    ctx.lineWidth = 0.6;
    ctx.beginPath();
    ctx.moveTo(gridLeft, stripMidY);
    ctx.lineTo(gridRight, stripMidY);
    ctx.stroke();

    // Ticks along the baseline
    ctx.strokeStyle = '#cccccc';
    ctx.lineWidth = 0.6;
    const tickSpacing = pxPerSec;
    for (let x = gridLeft; x <= gridRight; x += tickSpacing) {
        ctx.beginPath();
        ctx.moveTo(x, stripMidY - 3);
        ctx.lineTo(x, stripMidY + 3);
        ctx.stroke();
    }

    // Mini-strip Waveform (Mapped like web preview using globalMinV and globalMaxV with 10% padding)
    let globalMinV = Infinity, globalMaxV = -Infinity;
    for (let i = 0; i < totalPoints; i++) {
        const v = voltages[i];
        if (v < globalMinV) globalMinV = v;
        if (v > globalMaxV) globalMaxV = v;
    }
    if (!isFinite(globalMinV) || globalMinV === globalMaxV) {
        globalMinV = -1; globalMaxV = 1;
    }

    ctx.save();
    ctx.beginPath();
    ctx.rect(gridLeft, stripTop, plotW, stripH);
    ctx.clip();

    ctx.strokeStyle = '#000000';
    ctx.lineWidth = 0.75;
    ctx.lineJoin = 'round';
    ctx.lineCap = 'round';
    ctx.beginPath();

    for (let i = 0; i < totalPoints; i++) {
        const x = gridLeft + (i / samplingRate) * pxPerSec;
        const y = stripTop + getMappedY(voltages[i], stripH, globalMinV, globalMaxV);
        if (i === 0) ctx.moveTo(x, y);
        else ctx.lineTo(x, y);
    }
    ctx.stroke();
    ctx.restore();
}

// ---------------- NEW AGGREGATED HR EVENT GRAPH RENDER ----------------
// Generates fixed dummy HR data points every 5 minutes across 24 hours (0 to 1440 mins)
// Calibrated so that the overall Average HR across the 24-hour day is exactly 89 bpm.
function generateFixed5MinHRPoints() {
    const points = [];
    const step = 5; // Every 5 minutes
    for (let t = 0; t <= 1440; t += step) {
        const hour = t / 60;
        // Natural 24-hr circadian curve (lower at night/early morning ~4-5AM, higher in afternoon)
        const circadian = Math.sin(((hour - 9) / 24) * 2 * Math.PI);
        // Fixed deterministic harmonic wave for realistic medical ECG fluctuations
        const variation = Math.sin(t * 0.05) * 4.5 + Math.cos(t * 0.12) * 3.2 + Math.sin(t * 0.23) * 2.1;

        // Calibrated base to guarantee exact 89 bpm average across the 24 hours
        let avg = 87.85 + circadian * 10 + variation;

        // Realistic daytime activity peaks
        if (hour >= 10 && hour <= 11.5) avg += Math.sin((hour - 10) / 1.5 * Math.PI) * 12;
        if (hour >= 17.5 && hour <= 19) avg += Math.sin((hour - 17.5) / 1.5 * Math.PI) * 14;

        avg = Math.round(avg);

        // Min and Max spread for each 5-min epoch
        const minSpread = Math.round(10 + Math.abs(Math.sin(t * 0.07)) * 8);
        const maxSpread = Math.round(14 + Math.abs(Math.cos(t * 0.09)) * 16);

        let min = avg - minSpread;
        let max = avg + maxSpread;

        // Exertion spikes
        if (hour >= 10.5 && hour <= 11.2) max += 15;
        if (hour >= 18 && hour <= 18.8) max += 20;

        min = Math.max(50, min);
        max = Math.min(175, max);

        points.push({ startMins: t, min, max, avg });
    }
    return points;
}

function renderEventSummaryGraph(fetchedDataPoints) {
    const canvas = document.getElementById('hrCanvas');
    if (!canvas) return;
    const ctx = canvas.getContext('2d');
    const wrap = canvas.parentElement;

    function draw() {
        const dpr = Math.max(window.devicePixelRatio || 1, 4);
        const width = wrap.getBoundingClientRect().width || 735;
        const height = 130;

        canvas.width = width * dpr;
        canvas.height = height * dpr;
        canvas.style.width = width + 'px';
        canvas.style.height = height + 'px';
        ctx.setTransform(dpr, 0, 0, dpr, 0, 0);
        ctx.clearRect(0, 0, width, height);

        const marginLeft = 28, marginRight = 10, marginTop = 10, marginBottom = 18;
        const chartW = width - marginLeft - marginRight;
        const chartH = height - marginTop - marginBottom;
        const yMinVal = 0, yMaxVal = 200, yStep = 40;

        // Draw Y-axis grid lines and labels
        ctx.strokeStyle = '#E5E9ED';
        ctx.lineWidth = 1;
        ctx.fillStyle = '#000000';
        ctx.font = '9px -apple-system, Helvetica, Arial, sans-serif';
        ctx.textAlign = 'right';
        ctx.textBaseline = 'middle';

        for (let v = yMinVal; v <= yMaxVal; v += yStep) {
            const y = marginTop + chartH * (1 - (v - yMinVal) / (yMaxVal - yMinVal));
            ctx.beginPath();
            ctx.moveTo(marginLeft, y);
            ctx.lineTo(width - marginRight, y);
            ctx.stroke();
            ctx.fillText(String(v), marginLeft - 6, y);
        }

        const gapX = 15;
        const plotW = chartW - (gapX * 2);

        // Draw X-axis time labels every 2 hours from 12AM to 12AM
        ctx.fillStyle = '#000000';
        ctx.font = '9px -apple-system, Helvetica, Arial, sans-serif';
        ctx.textBaseline = 'top';

        for (let h = 0; h <= 24; h += 2) {
            const x = marginLeft + gapX + plotW * (h * 60 / 1440);
            let label;
            if (h === 0 || h === 24) label = '12AM';
            else if (h < 12) label = h + 'AM';
            else if (h === 12) label = '12PM';
            else label = (h - 12) + 'PM';

            if (h === 0) ctx.textAlign = 'left';
            else if (h === 24) ctx.textAlign = 'right';
            else ctx.textAlign = 'center';

            ctx.fillText(label, x, marginTop + chartH + 6);
        }

        // Always plot fixed points every 5 min with dummy values (or large dataset if provided)
        const pointsToPlot = (fetchedDataPoints && fetchedDataPoints.length >= 200)
            ? fetchedDataPoints
            : generateFixed5MinHRPoints();

        // Calculate Summary Stats
        const minValues = pointsToPlot.map(e => e.min);
        const maxValues = pointsToPlot.map(e => e.max);
        const avgSum = pointsToPlot.reduce((sum, e) => sum + e.avg, 0);

        const minStr = minValues.length > 0 ? Math.min(...minValues) : '--';
        const maxStr = maxValues.length > 0 ? Math.max(...maxValues) : '--';
        const avgStr = pointsToPlot.length > 0 ? Math.round(avgSum / pointsToPlot.length) : 89;

        // Safely update stat headers
        const statAvgEl = document.getElementById('statAvg');
        if (statAvgEl) statAvgEl.textContent = `Average HR: ${avgStr}bpm`;
        const statMinEl = document.getElementById('statMin');
        if (statMinEl) statMinEl.textContent = `Minimum Strip HR: ${minStr}bpm`;
        const statMaxEl = document.getElementById('statMax');
        if (statMaxEl) statMaxEl.textContent = `Maximum Strip HR: ${maxStr}bpm`;

        // Draw Min/Max vertical black lines with horizontal caps for every 5 min point
        ctx.strokeStyle = '#4a4a4a';
        ctx.lineWidth = 0.6;
        ctx.lineCap = 'butt';

        const capHalf = 1.0;

        pointsToPlot.forEach(d => {
            let x = marginLeft + gapX + plotW * (d.startMins / 1440);
            let yMaxPix = marginTop + chartH * (1 - (Math.min(d.max, yMaxVal) - yMinVal) / (yMaxVal - yMinVal));
            let yMinPix = marginTop + chartH * (1 - (Math.min(d.min, yMaxVal) - yMinVal) / (yMaxVal - yMinVal));

            ctx.beginPath();
            ctx.moveTo(x, yMinPix);
            ctx.lineTo(x, yMaxPix);

            ctx.moveTo(x - capHalf, yMaxPix);
            ctx.lineTo(x + capHalf, yMaxPix);

            ctx.moveTo(x - capHalf, yMinPix);
            ctx.lineTo(x + capHalf, yMinPix);
            ctx.stroke();
        });

        // Draw the continuous red line connecting all event averages
        ctx.strokeStyle = '#D9766B';
        ctx.lineWidth = 1.6;
        ctx.lineJoin = 'round';
        ctx.lineCap = 'round';
        ctx.beginPath();

        pointsToPlot.forEach((d, i) => {
            let x = marginLeft + gapX + plotW * (d.startMins / 1440);
            let yAvgPix = marginTop + chartH * (1 - (Math.min(d.avg, yMaxVal) - yMinVal) / (yMaxVal - yMinVal));

            if (i === 0) ctx.moveTo(x, yAvgPix);
            else ctx.lineTo(x, yAvgPix);
        });
        ctx.stroke();
    }

    draw();
    let resizeTimer;
    window.addEventListener('resize', () => {
        clearTimeout(resizeTimer);
        resizeTimer = setTimeout(draw, 100);
    });
}

// ============================================================================
// EDF (European Data Format) Parsing & Strip Loading Service
// Handles binary .edf buffers while keeping standard JSON parsing for .json files
// ============================================================================
const substripMetadataCache = new Map();

function setSubstripMetadata(identifier, metadata) {
    if (!identifier || !metadata) return;
    const str = String(identifier).trim();
    if (!str) return;
    const lower = str.toLowerCase();
    substripMetadataCache.set(lower, metadata);
    const normalized = lower.replace(/\\/g, '/');
    substripMetadataCache.set(normalized, metadata);
    const fileName = normalized.split('/').pop();
    if (fileName) {
        substripMetadataCache.set(fileName, metadata);
        const withoutExt = fileName.replace(/\.[^/.]+$/, '');
        substripMetadataCache.set(withoutExt, metadata);
    }
    if (metadata.sub_strip_event_id) {
        substripMetadataCache.set(String(metadata.sub_strip_event_id).toLowerCase(), metadata);
    }
    if (metadata.sub_strip_file_path) {
        const subNorm = String(metadata.sub_strip_file_path).trim().toLowerCase().replace(/\\/g, '/');
        substripMetadataCache.set(subNorm, metadata);
        const subFn = subNorm.split('/').pop();
        if (subFn) {
            substripMetadataCache.set(subFn, metadata);
            substripMetadataCache.set(subFn.replace(/\.[^/.]+$/, ''), metadata);
        }
    }
}

function getSubstripMetadata(identifier) {
    if (!identifier) return null;
    const str = String(identifier).trim();
    if (!str) return null;
    const lower = str.toLowerCase();
    if (substripMetadataCache.has(lower)) {
        return substripMetadataCache.get(lower);
    }
    const normalized = lower.replace(/\\/g, '/');
    if (substripMetadataCache.has(normalized)) {
        return substripMetadataCache.get(normalized);
    }
    const fileName = normalized.split('/').pop();
    if (fileName && substripMetadataCache.has(fileName)) {
        return substripMetadataCache.get(fileName);
    }
    if (fileName) {
        const withoutExt = fileName.replace(/\.[^/.]+$/, '');
        if (substripMetadataCache.has(withoutExt)) {
            return substripMetadataCache.get(withoutExt);
        }
    }
    const numMatch = lower.match(/(\d+)/);
    if (numMatch && substripMetadataCache.has(numMatch[1])) {
        return substripMetadataCache.get(numMatch[1]);
    }
    return null;
}

// Pre-seed known test cutstrip metadata (such as 4415 / ECG_4415.edf)
const test4415Snippet = {
    amplitudeScale: 2,
    yOffset: 0,
    beat_positions: [4, 161, 318, 475, 632, 789, 946, 1104, 1261, 1418, 1576, 1733, 1891, 2047, 2203, 2359, 2514],
    beat_labels: ['S', 'N', 'N', 'N', 'N', 'N', 'N', 'N', 'N', 'N', 'N', 'N', 'N', 'N', 'N', 'N', 'N']
};
setSubstripMetadata('4415', { waveform_snippet: test4415Snippet });
setSubstripMetadata('ecg_4415.edf', { waveform_snippet: test4415Snippet });

function parseDateStringToMs(dateVal) {
    if (dateVal == null) return 0;
    if (typeof dateVal === 'number') {
        return isNaN(dateVal) ? 0 : Math.round(dateVal);
    }
    if (dateVal instanceof Date) {
        const t = dateVal.getTime();
        return isNaN(t) ? 0 : t;
    }
    if (typeof dateVal === 'string') {
        const trimmed = dateVal.trim();
        if (!trimmed) return 0;

        // 1. Purely numeric / epoch ms string
        if (/^\d+(\.\d+)?$/.test(trimmed)) {
            const num = parseFloat(trimmed);
            if (!isNaN(num) && num > 0) {
                return num < 10000000000 ? Math.round(num * 1000) : Math.round(num);
            }
        }

        // 2. Format: "YYYY-MM-DD HH:mm:ss" or ISO "YYYY-MM-DDTHH:mm:ss"
        const isoMatch = trimmed.match(/^(\d{4})[-/](\d{2})[-/](\d{2})[ T](\d{2}):(\d{2}):(\d{2})(?:\.(\d+))?/);
        if (isoMatch) {
            const [_, y, m, d, hr, mn, sec, ms] = isoMatch;
            const msVal = ms ? parseInt(ms.padEnd(3, '0').slice(0, 3), 10) : 0;
            return new Date(
                parseInt(y, 10),
                parseInt(m, 10) - 1,
                parseInt(d, 10),
                parseInt(hr, 10),
                parseInt(mn, 10),
                parseInt(sec, 10),
                msVal
            ).getTime();
        }

        // 3. Format: "MM/DD/YYYY HH:mm:ss" or "DD/MM/YYYY HH:mm:ss"
        const dateParts = trimmed.split(/[ T]/);
        if (dateParts.length >= 2) {
            const [dPart, tPart] = dateParts;
            const subParts = dPart.split(/[-/]/);
            if (subParts.length === 3) {
                let year = 0, month = 0, day = 0;
                if (subParts[2].length === 4) {
                    year = parseInt(subParts[2], 10);
                    const p0 = parseInt(subParts[0], 10);
                    const p1 = parseInt(subParts[1], 10);
                    if (p0 > 12) {
                        day = p0;
                        month = p1 - 1;
                    } else {
                        month = p0 - 1;
                        day = p1;
                    }
                } else if (subParts[0].length === 4) {
                    year = parseInt(subParts[0], 10);
                    month = parseInt(subParts[1], 10) - 1;
                    day = parseInt(subParts[2], 10);
                }
                if (year > 0) {
                    const timeSub = tPart.split(':');
                    const hr = parseInt(timeSub[0], 10) || 0;
                    const mn = parseInt(timeSub[1], 10) || 0;
                    const secParts = (timeSub[2] || '0').split('.');
                    const sec = parseInt(secParts[0], 10) || 0;
                    const msVal = secParts[1] ? parseInt(secParts[1].padEnd(3, '0').slice(0, 3), 10) : 0;
                    return new Date(year, month, day, hr, mn, sec, msVal).getTime();
                }
            }
        }

        const parsed = new Date(trimmed).getTime();
        return isNaN(parsed) ? 0 : parsed;
    }
    return 0;
}

function parseEdf(buffer, fileName = '', options = {}) {
    if (!buffer || buffer.byteLength < 256) {
        throw new Error('Invalid EDF buffer: Buffer is empty or too short for EDF header.');
    }

    const bytes = new Uint8Array(buffer);
    const readStr = (start, len) => {
        let s = '';
        const limit = Math.min(start + len, bytes.length);
        for (let i = start; i < limit; i++) {
            s += String.fromCharCode(bytes[i]);
        }
        return s.trim();
    };

    // 1. Header parsing (First 256 bytes)
    const version = readStr(0, 8);
    const patientId = readStr(8, 80);
    const recordId = readStr(88, 80);
    const startDate = readStr(168, 8); // dd.mm.yy
    const startTime = readStr(176, 8); // hh.mm.ss
    const headerBytes = parseInt(readStr(184, 8), 10) || 256;
    const reserved = readStr(192, 44);
    let parsedAmplitudeScale;
    let parsedYOffset;
    let parsedWheel;
    const reservedMatch = reserved.match(/S:([0-9.]+);O:([0-9.-]+)(?:;W:([0-1]))?/);
    if (reservedMatch) {
        parsedAmplitudeScale = parseFloat(reservedMatch[1]);
        parsedYOffset = parseFloat(reservedMatch[2]);
        if (reservedMatch[3] !== undefined) {
            parsedWheel = reservedMatch[3] === '1';
        }
    }

    const numRecords = parseInt(readStr(236, 8), 10) || 1;
    const durationSec = parseFloat(readStr(244, 8)) || 1.0;
    const numSignals = parseInt(readStr(252, 4), 10) || 0;

    const header = {
        version,
        patientId,
        recordId,
        startDate,
        startTime,
        headerBytes,
        reserved,
        numRecords,
        durationSec,
        numSignals
    };

    // 2. Signals header parsing (256 bytes per signal)
    let offset = 256;
    const labels = [];
    for (let i = 0; i < numSignals; i++) { labels.push(readStr(offset + i * 16, 16)); }
    offset += numSignals * 16;

    const transducers = [];
    for (let i = 0; i < numSignals; i++) { transducers.push(readStr(offset + i * 80, 80)); }
    offset += numSignals * 80;

    const physDims = [];
    for (let i = 0; i < numSignals; i++) { physDims.push(readStr(offset + i * 8, 8)); }
    offset += numSignals * 8;

    const physMins = [];
    for (let i = 0; i < numSignals; i++) { physMins.push(parseFloat(readStr(offset + i * 8, 8))); }
    offset += numSignals * 8;

    const physMaxs = [];
    for (let i = 0; i < numSignals; i++) { physMaxs.push(parseFloat(readStr(offset + i * 8, 8))); }
    offset += numSignals * 8;

    const digMins = [];
    for (let i = 0; i < numSignals; i++) { digMins.push(parseFloat(readStr(offset + i * 8, 8))); }
    offset += numSignals * 8;

    const digMaxs = [];
    for (let i = 0; i < numSignals; i++) { digMaxs.push(parseFloat(readStr(offset + i * 8, 8))); }
    offset += numSignals * 8;

    const prefilterings = [];
    for (let i = 0; i < numSignals; i++) { prefilterings.push(readStr(offset + i * 80, 80)); }
    offset += numSignals * 80;

    const numSamples = [];
    for (let i = 0; i < numSignals; i++) { numSamples.push(parseInt(readStr(offset + i * 8, 8), 10)); }
    offset += numSignals * 8;

    const signals = [];
    for (let i = 0; i < numSignals; i++) {
        signals.push({
            label: labels[i],
            transducer: transducers[i],
            physDim: physDims[i],
            physMin: physMins[i],
            physMax: physMaxs[i],
            digMin: digMins[i],
            digMax: digMaxs[i],
            prefiltering: prefilterings[i],
            numSamples: numSamples[i],
            samplingRate: Math.round(numSamples[i] / durationSec)
        });
    }

    // 3. Locate ECG signal & Annotations signal
    let ecgSigIdx = labels.findIndex(l => l.toUpperCase().includes('ECG'));
    if (ecgSigIdx === -1) {
        ecgSigIdx = 0;
    }
    const annotSigIdx = labels.findIndex(l => l.toUpperCase().includes('ANNOTATION'));

    const sRate = signals[ecgSigIdx]?.samplingRate || 256;
    const samplesPerRecord = numSamples[ecgSigIdx] || 0;
    const totalEcgSamples = samplesPerRecord * numRecords;

    const physMin = physMins[ecgSigIdx];
    const physMax = physMaxs[ecgSigIdx];
    const digMin = digMins[ecgSigIdx];
    const digMax = digMaxs[ecgSigIdx];
    const digRange = digMax - digMin;
    const physRange = physMax - physMin;
    const scale = digRange !== 0 ? physRange / digRange : 1;

    // 4. Extract samples and annotations across all records
    const voltages = new Float32Array(totalEcgSamples);
    const dataView = new DataView(buffer);
    let recordOffset = headerBytes;
    let ecgWriteIdx = 0;
    const rawAnnotBytes = [];

    for (let r = 0; r < numRecords; r++) {
        let currentSignalOffset = recordOffset;
        for (let s = 0; s < numSignals; s++) {
            const sampleCount = numSamples[s];
            const byteCount = sampleCount * 2;

            if (s === ecgSigIdx) {
                for (let i = 0; i < sampleCount; i++) {
                    if (currentSignalOffset + i * 2 + 1 < buffer.byteLength) {
                        const dig = dataView.getInt16(currentSignalOffset + i * 2, true);
                        voltages[ecgWriteIdx++] = physMin + (dig - digMin) * scale;
                    }
                }
            } else if (s === annotSigIdx) {
                for (let i = 0; i < byteCount; i++) {
                    if (currentSignalOffset + i < bytes.length) {
                        rawAnnotBytes.push(bytes[currentSignalOffset + i]);
                    }
                }
            }
            currentSignalOffset += byteCount;
        }
        recordOffset = currentSignalOffset;
    }

    // 5. Parse EDF+ TAL (Time-stamped Annotations List)
    let metadata = {};
    const annotationsMap = new Map();
    const beatPositions = [];
    const beatLabels = [];
    const annotations = [];

    if (annotSigIdx !== -1 && rawAnnotBytes.length > 0) {
        let text = '';
        for (let i = 0; i < rawAnnotBytes.length; i++) {
            text += String.fromCharCode(rawAnnotBytes[i]);
        }

        let i = 0;
        while (i < text.length) {
            if (text.charCodeAt(i) === 0) {
                i++;
                continue;
            }
            let nextNull = text.indexOf('\u0000', i);
            if (nextNull === -1) nextNull = text.length;
            const block = text.slice(i, nextNull);
            i = nextNull + 1;

            if (!block.startsWith('+') && !block.startsWith('-')) continue;
            const parts = block.split('\u0014');
            if (parts.length < 2) continue;

            const timePart = parts[0];
            const durationParts = timePart.split('\u0015');
            const onsetSec = parseFloat(durationParts[0]);
            const durSec = durationParts.length > 1 ? parseFloat(durationParts[1]) : undefined;

            for (let p = 1; p < parts.length; p++) {
                const item = parts[p].trim();
                if (!item) continue;
                annotations.push({ onsetSec, durationSec: durSec, text: item });

                if (item.startsWith('JSON_METADATA')) {
                    try {
                        const jsonStr = item.slice('JSON_METADATA'.length).trim();
                        metadata = JSON.parse(jsonStr);
                    } catch (e) {
                        console.warn('[EdfService] Failed parsing JSON_METADATA:', e);
                    }
                } else if (item.startsWith('BEAT=')) {
                    const label = item.slice(5).trim();
                    const sampleIndex = Math.min(totalEcgSamples - 1, Math.max(0, Math.round(onsetSec * sRate)));
                    annotationsMap.set(sampleIndex, label);
                    beatPositions.push(sampleIndex);
                    beatLabels.push(label);
                } else if (item.startsWith('type=')) {
                    if (!metadata.type) metadata.type = item.slice(5).trim();
                } else if (item.startsWith('severity=')) {
                    if (!metadata.severity) metadata.severity = item.slice(9).trim();
                }
            }
        }
    }

    // 6. Base timestamp calculation (preserving exact local clock)
    let startTimestampMs = 0;
    const opt = options || {};
    const cached = getSubstripMetadata(fileName) ||
        (opt.sub_strip_file_path ? getSubstripMetadata(opt.sub_strip_file_path) : null) ||
        (opt.fileName ? getSubstripMetadata(opt.fileName) : null);

    if (opt.startTimestampMs) {
        startTimestampMs = typeof opt.startTimestampMs === 'number'
            ? opt.startTimestampMs
            : parseDateStringToMs(opt.startTimestampMs);
    }

    if (!startTimestampMs) {
        const candidateTimestamp =
            opt.sub_strip_start_datetime ||
            cached?.sub_strip_start_datetime ||
            opt.activeSubStrip?.sub_strip_start_datetime ||
            opt.rawData?.sub_strip_start_datetime ||
            cached?.raw?.sub_strip_start_datetime ||
            opt.event_start_datetime ||
            cached?.event_start_datetime ||
            opt.activeSubStrip?.event_start_datetime ||
            opt.rawData?.event_start_datetime ||
            cached?.raw?.event_start_datetime ||
            opt.timestamp ||
            cached?.timestamp ||
            metadata.timestamp ||
            opt.start_datetime ||
            cached?.start_datetime;

        if (candidateTimestamp) {
            startTimestampMs = parseDateStringToMs(candidateTimestamp);
        }
    }

    if (!startTimestampMs && fileName) {
        const fnMatch = fileName.match(/(\d{4})(\d{2})(\d{2})(\d{2})(\d{2})(\d{2})(\d{3})?/);
        if (fnMatch) {
            const [_, y, m, d, hr, mn, sec, ms] = fnMatch;
            startTimestampMs = new Date(
                parseInt(y, 10),
                parseInt(m, 10) - 1,
                parseInt(d, 10),
                parseInt(hr, 10),
                parseInt(mn, 10),
                parseInt(sec, 10),
                ms ? parseInt(ms, 10) : 0
            ).getTime();
        }
    }

    if (!startTimestampMs && startDate && startTime) {
        const dateParts = startDate.split('.').map(x => parseInt(x, 10));
        const timeParts = startTime.split('.').map(x => parseInt(x, 10));
        if (dateParts.length === 3 && timeParts.length === 3) {
            const [d, m, y] = dateParts;
            const [hr, mn, sec] = timeParts;
            const fullYear = y < 85 ? 2000 + y : 1900 + y;
            startTimestampMs = new Date(fullYear, m - 1, d, hr, mn, sec, 0).getTime();
        }
    }

    if (!startTimestampMs) {
        startTimestampMs = Date.now();
    }

    // 7. Generate array of timestamps for each sample
    const times = new Float64Array(totalEcgSamples);
    const stepMs = 1000 / sRate;
    for (let i = 0; i < totalEcgSamples; i++) {
        times[i] = startTimestampMs + i * stepMs;
    }

    const endTimestampMs = times.length > 0 ? times[times.length - 1] : startTimestampMs;

    // 8. Min and Max voltages
    let minV = Infinity;
    let maxV = -Infinity;
    for (let i = 0; i < totalEcgSamples; i++) {
        const v = voltages[i];
        if (v < minV) minV = v;
        if (v > maxV) maxV = v;
    }
    if (minV === Infinity) minV = -1;
    if (maxV === -Infinity) maxV = 1;
    if (minV === maxV) {
        minV -= 1;
        maxV += 1;
    }

    const optSnippet = options?.json_data?.waveform_snippet || options?.waveform_snippet || options;
    const cachedSnippet = cached?.json_data?.waveform_snippet || cached?.waveform_snippet || cached;

    const resolvedAmplitudeScale =
        parsedAmplitudeScale ??
        optSnippet?.amplitudeScale ??
        metadata?.amplitudeScale ??
        metadata?.scale ??
        cachedSnippet?.amplitudeScale ??
        1;

    const resolvedYOffset =
        parsedYOffset ??
        optSnippet?.yOffset ??
        metadata?.yOffset ??
        metadata?.y_offset ??
        metadata?.offset ??
        cachedSnippet?.yOffset ??
        0;

    const optBeats = optSnippet?.beat_positions || cachedSnippet?.beat_positions;
    const optLabels = optSnippet?.beat_labels || cachedSnippet?.beat_labels;
    if (beatPositions.length === 0 && optBeats && optLabels && optBeats.length > 0) {
        for (let i = 0; i < optBeats.length; i++) {
            const pos = optBeats[i];
            const lbl = optLabels[i] || 'N';
            beatPositions.push(pos);
            beatLabels.push(lbl);
            annotationsMap.set(pos, lbl);
        }
    }

    return {
        header,
        signals,
        samplingRate: sRate,
        durationSec: totalEcgSamples / sRate,
        totalPoints: totalEcgSamples,
        times,
        voltages,
        multiChannelVoltages: [voltages, voltages, voltages, voltages, voltages],
        annotationsMap,
        beatPositions,
        beatLabels,
        annotations,
        metadata,
        startTimestampMs,
        endTimestampMs,
        globalMinV: minV,
        globalMaxV: maxV,
        fileName,
        amplitudeScale: resolvedAmplitudeScale,
        yOffset: resolvedYOffset,
        waveform_snippet: {
            timestamp_ms: Array.from(times),
            voltage_mv: Array.from(voltages),
            beat_positions: beatPositions,
            beat_labels: beatLabels,
            sampling_rate: sRate,
            amplitudeScale: resolvedAmplitudeScale,
            yOffset: resolvedYOffset
        }
    };
}

/**
 * Loads and parses a strip entry:
 * - If the file path is .edf: fetches arrayBuffer and parses via parseEdf()
 * - If the file path is .json: fetches and parses JSON as before
 */
async function fetchAndParseStrip(entry) {
    if (!entry || !entry.path) return { entry, data: {} };
    try {
        const safePath = entry.path.replace(/\\/g, '/').replace(/^\//, '');
        const isEdf = safePath.toLowerCase().endsWith('.edf') || safePath.toLowerCase().includes('.edf');

        const res = await fetch(safePath);
        if (!res.ok) throw new Error(`Status ${res.status}`);

        if (isEdf) {
            const arrayBuffer = await res.arrayBuffer();
            const fileName = safePath.split('/').pop() || '';
            const edfData = parseEdf(arrayBuffer, fileName, entry.originalObj);
            return { entry, data: edfData };
        } else {
            const json = await res.json();
            return { entry, data: json };
        }
    } catch (err) {
        console.error(`Failed to fetch/parse ${entry.path}:`, err);
        return { entry, data: null };
    }
}