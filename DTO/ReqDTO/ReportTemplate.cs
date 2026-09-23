using elr_API.Common;

namespace elr_API.DTO.ReqDTO
{
    public class ReportTemplate
    {
    }



    public class PatientReportPdfDTO
    {
        public PatientPdfDTO patient_details { get; set; }

        public EventSummaryPdfDTO event_summary { get; set; }

        public ArrhythmiaPdfDTO arrhythmias_present { get; set; }

        public AF_BurdenDTO aF_Burden { get; set; }

        public PVC_BurdenDTO pvc { get; set; }

        public PAC_BurdenDTO pac { get; set; }

        public PauseDTO pause { get; set; }
        public List<ReportStripPdfDTO> strip_list { get; set; }

        public List<ReportQerPdfDTO> qer_list { get; set; }


    }

    public class ReportQerPdfDTO
    {
        public long queued_events_records_id { get; set; }
        public long patient_enrollment_id { get; set; }

        public DateTime? event_start_datetime { get; set; }
        public DateTime? event_end_datetime { get; set; }

        public string trigger_type { get; set; }
        public int? bpm { get; set; }

        public string event_file_path { get; set; }

        internal static string get_qer_data()
        {
            return @"
            SELECT
                qer.queued_events_records_id,
                qer.patient_enrollment_id,
                qer.event_start_datetime,
                qer.event_end_datetime,
                qer.trigger_type,
                qer.bpm,
                qer.event_file_path
            FROM tbl_patient_report pr
            INNER JOIN tbl_queued_events_records qer
                ON qer.patient_enrollment_id = pr.patient_enrollment_id
            WHERE pr.patient_report_id = @patient_report_id
            ORDER BY qer.event_start_datetime;
        ";
        }
    }


    public class ReportStripPdfDTO
    {
        public long patient_report_generate_id { get; set; }
        public long patient_report_id { get; set; }
        public long queued_events_records_id { get; set; }
        public long sub_strip_event_id { get; set; }

        public DateTime? event_start_datetime { get; set; }
        public DateTime? event_end_datetime { get; set; }

        public string event_file_path { get; set; }

        public string sub_strip_file_path { get; set; }

        // DB value is Unix timestamp string
        public string sub_strip_start_datetime { get; set; }
        public string sub_strip_end_datetime { get; set; }

        public List<FindingsNameReportDTO> finding_list { get; set; }

        internal static string get_strip_data()
        {
            return @"
        SELECT
            a.patient_report_generate_id,
            a.patient_report_id,
            a.queued_events_records_id,
            a.sub_strip_event_id,

            qer.event_start_datetime,
            qer.event_end_datetime,
            qer.event_file_path,

            sse.sub_strip_file_path,
            sse.sub_strip_start_datetime,
            sse.sub_strip_end_datetime

        FROM tbl_patient_report_generate AS a

        INNER JOIN tbl_patient_report AS pr
            ON pr.patient_report_id = a.patient_report_id

        INNER JOIN tbl_patient_enrollment AS pe
            ON pe.patient_enrollment_id = pr.patient_enrollment_id

        INNER JOIN tbl_queued_events_records AS qer
            ON qer.queued_events_records_id = a.queued_events_records_id

        INNER JOIN tbl_sub_strip_event AS sse
            ON sse.sub_strip_event_id = a.sub_strip_event_id

        WHERE a.patient_report_id = @patient_report_id
          AND a.is_active = 1
          AND sse.is_active = 1

          AND qer.event_start_datetime >= pe.actual_hookup

          AND qer.event_end_datetime <= DATEADD(
                HOUR,
                24,
                pe.actual_hookup
              )

        ORDER BY
            qer.event_start_datetime,
            a.sub_strip_event_id;";
        }
    }

    public class FindingsNameReportDTO
    {
        public long event_finding_id { get; set; }
        public string finding_name { get; set; }
        public decimal? value1 { get; set; }
        public decimal? value2 { get; set; }

        internal static string get_finding_data()
        {
            return @"
            SELECT
                b.event_finding_id,
                b.value1,
                b.value2,
                d.finding_name
            FROM tbl_sub_strip_event_finding AS b
            INNER JOIN tbl_event_finding_mast AS d
                ON d.event_finding_id = b.event_finding_id
            WHERE b.sub_strip_event_id = @sub_strip_event_id
              AND b.is_active = 1
            ORDER BY b.event_finding_id;";
        }
    }

    //internal static string get_strip_data()
    //{
    //    return $@"
    //SELECT
    //    a.patient_report_generate_id,
    //    a.patient_report_id,
    //    a.queued_events_records_id,
    //    a.sub_strip_event_id,

    //    qer.event_start_datetime,
    //    qer.event_end_datetime,
    //    qer.event_file_path,

    //    sse.sub_strip_file_path,
    //    sse.sub_strip_start_datetime,
    //    sse.sub_strip_end_datetime,

    //    STRING_AGG(
    //        d.finding_name,
    //        ', '
    //    ) AS finding_names

    //FROM {Tbl.patient_report_generate} AS a

    //INNER JOIN {Tbl.patient_report} AS pr
    //    ON pr.patient_report_id = a.patient_report_id

    //INNER JOIN {Tbl.patient_enrollment} AS pe
    //    ON pe.patient_enrollment_id =
    //       pr.patient_enrollment_id

    //INNER JOIN {Tbl.queued_events_records} AS qer
    //    ON qer.queued_events_records_id =
    //       a.queued_events_records_id

    //INNER JOIN {Tbl.sub_strip_event} AS sse
    //    ON sse.sub_strip_event_id =
    //       a.sub_strip_event_id

    //LEFT JOIN {Tbl.sub_strip_event_finding} AS b
    //    ON b.sub_strip_event_id =
    //       a.sub_strip_event_id
    //    AND b.is_active = 1

    //LEFT JOIN {Tbl.event_finding_mast} AS d
    //    ON d.event_finding_id =
    //       b.event_finding_id

    //WHERE a.patient_report_id = @patient_report_id

    //  AND a.is_active = 1

    //  AND sse.is_active = 1

    //  AND qer.event_start_datetime >=
    //      pe.actual_hookup

    //  AND qer.event_end_datetime <=
    //      DATEADD(
    //          HOUR,
    //          24,
    //          pe.actual_hookup
    //      )

    //GROUP BY
    //    a.patient_report_generate_id,
    //    a.patient_report_id,
    //    a.queued_events_records_id,
    //    a.sub_strip_event_id,

    //    qer.event_start_datetime,
    //    qer.event_end_datetime,
    //    qer.event_file_path,

    //    sse.sub_strip_file_path,
    //    sse.sub_strip_start_datetime,
    //    sse.sub_strip_end_datetime

    //ORDER BY
    //    qer.event_start_datetime,
    //    a.sub_strip_event_id";
    //}




    public class PauseDTO
    {
        public long am { get; set; }
        public long am_longest_duration { get; set; }

        public long pm { get; set; }
        public long pm_longest_duration { get; set; }

        public long total { get; set; }
        public long longest_duration { get; set; }

        public decimal burden { get; set; }

        internal static string get_pause_data()
        {
            return $@"

        SELECT

            -- =========================================
            -- AM COUNT
            -- =========================================

            COUNT(CASE
                WHEN CAST(pause_data.event_start_datetime AS TIME)
                     < '12:00:00'
                THEN 1
            END) AS am,


            -- =========================================
            -- AM LONGEST DURATION
            -- =========================================

            ISNULL(
                MAX(CASE
                    WHEN CAST(
                        pause_data.event_start_datetime AS TIME
                    ) < '12:00:00'
                    THEN pause_data.duration_minutes
                END),
                0
            ) AS am_longest_duration,


            -- =========================================
            -- PM COUNT
            -- =========================================

            COUNT(CASE
                WHEN CAST(pause_data.event_start_datetime AS TIME)
                     >= '12:00:00'
                THEN 1
            END) AS pm,


            -- =========================================
            -- PM LONGEST DURATION
            -- =========================================

            ISNULL(
                MAX(CASE
                    WHEN CAST(
                        pause_data.event_start_datetime AS TIME
                    ) >= '12:00:00'
                    THEN pause_data.duration_minutes
                END),
                0
            ) AS pm_longest_duration,


            -- =========================================
            -- TOTAL COUNT
            -- =========================================

            COUNT(*) AS total,


            -- =========================================
            -- TOTAL LONGEST DURATION
            -- =========================================

            ISNULL(
                MAX(pause_data.duration_minutes),
                0
            ) AS longest_duration,


            -- =========================================
            -- TOTAL PAUSE BURDEN
            --
            -- SUM OF ALL PAUSE FILE DURATIONS
            -- / 1440 MINUTES
            -- * 100
            -- =========================================

            CAST(
                (
                    (
                        CAST(
                            SUM(
                                pause_data.duration_seconds
                            )
                            AS DECIMAL(18,2)
                        ) / 60.0
                    )
                    /
                    NULLIF(
                        DATEDIFF(
                            MINUTE,
                            MAX(
                                pause_data.report_started_datetime
                            ),
                            MAX(
                                pause_data.report_due_datetime
                            )
                        ),
                        0
                    )
                ) * 100
                AS DECIMAL(10,2)
            ) AS burden


        FROM
        (
            SELECT DISTINCT

                -- One QER = One Pause file
                qe.queued_events_records_id,

                qe.event_start_datetime,

                qe.event_end_datetime,


                -- Duration in seconds
                DATEDIFF(
                    SECOND,
                    TRY_CONVERT(
                        DATETIME,
                        qe.event_start_datetime
                    ),
                    TRY_CONVERT(
                        DATETIME,
                        qe.event_end_datetime
                    )
                ) AS duration_seconds,


                -- Duration in minutes
                DATEDIFF(
                    MINUTE,
                    TRY_CONVERT(
                        DATETIME,
                        qe.event_start_datetime
                    ),
                    TRY_CONVERT(
                        DATETIME,
                        qe.event_end_datetime
                    )
                ) AS duration_minutes,


                pr.report_started_datetime,

                pr.report_due_datetime


            FROM tbl_patient_report pr


            INNER JOIN tbl_queued_events_records qe
                ON qe.patient_enrollment_id =
                   pr.patient_enrollment_id


            INNER JOIN tbl_sub_strip_event_finding sse
                ON sse.queued_events_records_id =
                   qe.queued_events_records_id


            WHERE

                pr.patient_report_id =
                    @patient_report_id


                AND pr.report_status IN
                (
                    'inprogress',
                    'completed'
                )


                AND qe.event_status =
                    'completed'


                AND qe.completed_by IS NOT NULL


                AND sse.is_active =
                    1


                AND sse.sub_strip_event_id IS NULL


                -- =========================================
                -- PAUSE FINDINGS
                -- =========================================

                AND sse.event_finding_id IN
                (
                    73,
                    74,
                    75,
                    76,
                    77,
                    78,
                    79
                )


                -- =========================================
                -- 24 HOUR REPORT WINDOW
                -- =========================================

                AND qe.event_start_datetime >=
                    pr.report_started_datetime

                AND qe.event_start_datetime <
                    pr.report_due_datetime


                -- =========================================
                -- DETECTION COMPLETED
                -- =========================================

                AND EXISTS
                (
                    SELECT 1
                    FROM tbl_detection d

                    WHERE
                        d.queued_events_records_id =
                            qe.queued_events_records_id

                        AND d.sub_strip_event_id IS NULL

                        AND d.is_active = 1

                        AND d.detection_status IN
                        (
                            'submit',
                            'confirm-submit'
                        )
                )

        ) pause_data;

    ";
        }



    }




    public class PAC_BurdenDTO
    {
        public long am { get; set; }
        public decimal am_burden { get; set; }

        public long pm { get; set; }
        public decimal pm_burden { get; set; }

        public long total { get; set; }
        public decimal total_burden { get; set; }

        internal static string get_pac_data()
        {
            return $@"

        SELECT

            -- =========================
            -- AM COUNT
            -- =========================

            COUNT(CASE
                WHEN CAST(pac_data.event_start_datetime AS TIME)
                     < '12:00:00'
                THEN 1
            END) AS am,


            -- =========================
            -- AM BURDEN
            -- =========================

            CAST(
                (
                    SUM(
                        CASE
                            WHEN CAST(
                                pac_data.event_start_datetime AS TIME
                            ) < '12:00:00'
                            THEN pac_data.duration_seconds
                            ELSE 0
                        END
                    ) / 60.0
                )
                /
                NULLIF(
                    DATEDIFF(
                        MINUTE,
                        MAX(pac_data.report_started_datetime),
                        MAX(pac_data.report_due_datetime)
                    ),
                    0
                )
                * 100
                AS DECIMAL(10,2)
            ) AS am_burden,


            -- =========================
            -- PM COUNT
            -- =========================

            COUNT(CASE
                WHEN CAST(pac_data.event_start_datetime AS TIME)
                     >= '12:00:00'
                THEN 1
            END) AS pm,


            -- =========================
            -- PM BURDEN
            -- =========================

            CAST(
                (
                    SUM(
                        CASE
                            WHEN CAST(
                                pac_data.event_start_datetime AS TIME
                            ) >= '12:00:00'
                            THEN pac_data.duration_seconds
                            ELSE 0
                        END
                    ) / 60.0
                )
                /
                NULLIF(
                    DATEDIFF(
                        MINUTE,
                        MAX(pac_data.report_started_datetime),
                        MAX(pac_data.report_due_datetime)
                    ),
                    0
                )
                * 100
                AS DECIMAL(10,2)
            ) AS pm_burden,


            -- =========================
            -- TOTAL COUNT
            -- =========================

            COUNT(*) AS total,


            -- =========================
            -- TOTAL BURDEN
            -- =========================

            CAST(
                (
                    SUM(pac_data.duration_seconds) / 60.0
                )
                /
                NULLIF(
                    DATEDIFF(
                        MINUTE,
                        MAX(pac_data.report_started_datetime),
                        MAX(pac_data.report_due_datetime)
                    ),
                    0
                )
                * 100
                AS DECIMAL(10,2)
            ) AS total_burden


        FROM
        (
            SELECT DISTINCT

                qe.queued_events_records_id,

                qe.event_start_datetime,

                qe.event_end_datetime,

                DATEDIFF(
                    SECOND,
                    TRY_CONVERT(
                        DATETIME,
                        qe.event_start_datetime
                    ),
                    TRY_CONVERT(
                        DATETIME,
                        qe.event_end_datetime
                    )
                ) AS duration_seconds,

                pr.report_started_datetime,

                pr.report_due_datetime

            FROM tbl_patient_report pr

            INNER JOIN tbl_queued_events_records qe
                ON qe.patient_enrollment_id =
                   pr.patient_enrollment_id

            INNER JOIN tbl_sub_strip_event_finding sse
                ON sse.queued_events_records_id =
                   qe.queued_events_records_id

            WHERE

                pr.patient_report_id = @patient_report_id

                AND pr.report_status IN
                (
                    'inprogress',
                    'completed'
                )

                AND qe.event_status = 'completed'

                AND qe.completed_by IS NOT NULL

                AND sse.is_active = 1

                AND sse.sub_strip_event_id IS NULL

                -- PAC
                AND sse.event_finding_id = 11

                -- 24 hour report window
                AND qe.event_start_datetime >=
                    pr.report_started_datetime

                AND qe.event_start_datetime <
                    pr.report_due_datetime

                -- Detection completed
                AND EXISTS
                (
                    SELECT 1
                    FROM tbl_detection d

                    WHERE
                        d.queued_events_records_id =
                            qe.queued_events_records_id

                        AND d.sub_strip_event_id IS NULL

                        AND d.is_active = 1

                        AND d.detection_status IN
                        (
                            'submit',
                            'confirm-submit'
                        )
                )

        ) pac_data;

    ";
        }
    }

    public class PVC_BurdenDTO
    {
        public long am { get; set; }
        public decimal am_burden { get; set; }

        public long pm { get; set; }
        public decimal pm_burden { get; set; }

        public long total { get; set; }
        public decimal total_burden { get; set; }

        internal static string get_pvc_data()
        {
            return $@"

        SELECT
            COUNT(CASE
                WHEN CAST(pvc_data.event_start_datetime AS TIME) < '12:00:00'
                THEN 1
            END) AS am,

            CAST(
                (
                    SUM(CASE
                        WHEN CAST(pvc_data.event_start_datetime AS TIME) < '12:00:00'
                        THEN pvc_data.duration_seconds
                        ELSE 0
                    END) / 60.0
                )
                /
                NULLIF(
                    DATEDIFF(
                        MINUTE,
                        MAX(pvc_data.report_started_datetime),
                        MAX(pvc_data.report_due_datetime)
                    ),
                    0
                )
                * 100
                AS DECIMAL(10,2)
            ) AS am_burden,


            COUNT(CASE
                WHEN CAST(pvc_data.event_start_datetime AS TIME) >= '12:00:00'
                THEN 1
            END) AS pm,

            CAST(
                (
                    SUM(CASE
                        WHEN CAST(pvc_data.event_start_datetime AS TIME) >= '12:00:00'
                        THEN pvc_data.duration_seconds
                        ELSE 0
                    END) / 60.0
                )
                /
                NULLIF(
                    DATEDIFF(
                        MINUTE,
                        MAX(pvc_data.report_started_datetime),
                        MAX(pvc_data.report_due_datetime)
                    ),
                    0
                )
                * 100
                AS DECIMAL(10,2)
            ) AS pm_burden,


            COUNT(*) AS total,

            CAST(
                (
                    SUM(pvc_data.duration_seconds) / 60.0
                )
                /
                NULLIF(
                    DATEDIFF(
                        MINUTE,
                        MAX(pvc_data.report_started_datetime),
                        MAX(pvc_data.report_due_datetime)
                    ),
                    0
                )
                * 100
                AS DECIMAL(10,2)
            ) AS total_burden


        FROM
        (
            SELECT DISTINCT
                qe.queued_events_records_id,

                qe.event_start_datetime,
                qe.event_end_datetime,

                DATEDIFF(
                    SECOND,
                    TRY_CONVERT(
                        DATETIME,
                        qe.event_start_datetime
                    ),
                    TRY_CONVERT(
                        DATETIME,
                        qe.event_end_datetime
                    )
                ) AS duration_seconds,

                pr.report_started_datetime,
                pr.report_due_datetime

            FROM tbl_patient_report pr

            INNER JOIN tbl_queued_events_records qe
                ON qe.patient_enrollment_id =
                   pr.patient_enrollment_id

            INNER JOIN tbl_sub_strip_event_finding sse
                ON sse.queued_events_records_id =
                   qe.queued_events_records_id

            WHERE
                pr.patient_report_id = @patient_report_id

                AND pr.report_status IN
                (
                    'inprogress',
                    'completed'
                )

                AND qe.event_status = 'completed'

                AND qe.completed_by IS NOT NULL

                AND sse.is_active = 1

                AND sse.sub_strip_event_id IS NULL

                AND sse.event_finding_id = 19

                -- 24 hour report window
                AND qe.event_start_datetime >=
                    pr.report_started_datetime

                AND qe.event_start_datetime <
                    pr.report_due_datetime

                -- Detection completed
                AND EXISTS
                (
                    SELECT 1
                    FROM tbl_detection d

                    WHERE
                        d.queued_events_records_id =
                            qe.queued_events_records_id

                        AND d.sub_strip_event_id IS NULL

                        AND d.is_active = 1

                        AND d.detection_status IN
                        (
                            'submit',
                            'confirm-submit'
                        )
                )
        ) pvc_data;

    ";
        }
    }

    public class AF_BurdenDTO
    {
        public decimal burden { get; set; }
        public long episode_count { get; set; }
        public long high_hr { get; set; }
        public long longest_af { get; set; }

        #region Query

        internal static string get_af_burden_data()
        {
            return $@"
            SELECT

                -- =========================================
                -- AF FILE / EPISODE COUNT
                -- =========================================

                COUNT(*) AS episode_count,


                -- =========================================
                -- HIGHEST HR FROM ALL AF FILES
                -- =========================================

                ISNULL(
                    MAX(af_data.bpm),
                    0
                ) AS high_hr,


                -- =========================================
                -- LONGEST AF FILE DURATION
                -- =========================================

                ISNULL(
                    MAX(af_data.duration_minutes),
                    0
                ) AS longest_af,


                -- =========================================
                -- AF BURDEN
                --
                -- SUM OF ALL AF FILE DURATIONS
                -- /
                -- 24 HOUR REPORT DURATION
                -- *
                -- 100
                -- =========================================

                CAST(
                    (
                        (
                            CAST(
                                SUM(af_data.duration_seconds)
                                AS DECIMAL(18,2)
                            ) / 60.0
                        )
                        /
                        NULLIF(
                            DATEDIFF(
                                MINUTE,
                                MAX(af_data.report_started_datetime),
                                MAX(af_data.report_due_datetime)
                            ),
                            0
                        )
                    ) * 100
                    AS DECIMAL(10,2)
                ) AS burden


            FROM
            (
                SELECT DISTINCT

                    -- One QER = One AF file
                    qe.queued_events_records_id,

                    qe.bpm,


                    -- File duration in seconds
                    DATEDIFF(
                        SECOND,
                        TRY_CONVERT(
                            DATETIME,
                            qe.event_start_datetime
                        ),
                        TRY_CONVERT(
                            DATETIME,
                            qe.event_end_datetime
                        )
                    ) AS duration_seconds,


                    -- File duration in minutes
                    DATEDIFF(
                        MINUTE,
                        TRY_CONVERT(
                            DATETIME,
                            qe.event_start_datetime
                        ),
                        TRY_CONVERT(
                            DATETIME,
                            qe.event_end_datetime
                        )
                    ) AS duration_minutes,


                    pr.report_started_datetime,

                    pr.report_due_datetime


                FROM tbl_patient_report pr


                INNER JOIN tbl_queued_events_records qe
                    ON qe.patient_enrollment_id =
                       pr.patient_enrollment_id


                INNER JOIN tbl_sub_strip_event_finding sse
                    ON sse.queued_events_records_id =
                       qe.queued_events_records_id


                WHERE

                    pr.patient_report_id =
                        @patient_report_id


                    AND pr.report_status IN
                    (
                        'inprogress',
                        'completed'
                    )


                    AND qe.event_status =
                        'completed'


                    AND qe.completed_by IS NOT NULL


                    AND sse.is_active =
                        1


                    AND sse.sub_strip_event_id IS NULL


                    -- =========================================
                    -- ALL AF FILES HAVE FINDING ID 35
                    -- =========================================

                    AND sse.event_finding_id = 35


                    -- =========================================
                    -- REPORT WINDOW
                    -- =========================================

                    AND qe.event_start_datetime <
                        pr.report_due_datetime

                    AND qe.event_end_datetime >
                        pr.report_started_datetime


                    -- =========================================
                    -- DETECTION COMPLETED
                    -- =========================================

                    AND EXISTS
                    (
                        SELECT 1
                        FROM tbl_detection d

                        WHERE
                            d.queued_events_records_id =
                                qe.queued_events_records_id

                            AND d.sub_strip_event_id IS NULL

                            AND d.is_active = 1

                            AND d.detection_status IN
                            (
                                'submit',
                                'confirm-submit'
                            )
                    )

            ) af_data;
        ";
        }

        #endregion
    }

    public class ArrhythmiaPdfDTO
    {
        public string af { get; set; }
        public string svt { get; set; }
        public string pause { get; set; }
        public string heart_block { get; set; }
        public string vt { get; set; }

        #region Query
        internal static string get_arrhythmia_data()
        {
            return $@"
                    
                    SELECT
                        COUNT(CASE
                            WHEN sse.event_finding_id = 35
                            THEN 1
                        END) AS af,

                        COUNT(CASE
                            WHEN sse.event_finding_id = 34
                            THEN 1
                        END) AS svt,

                        COUNT(CASE
                            WHEN sse.event_finding_id IN (73,74,75,76,77,78,79)
                            THEN 1
                        END) AS pause,

                        COUNT(CASE
                            WHEN sse.event_finding_id IN (57,58,59,60,61,62,63)
                            THEN 1
                        END) AS heart_block,

                        COUNT(CASE
                            WHEN sse.event_finding_id = 48
                            THEN 1
                        END) AS vt

                    FROM tbl_patient_report pr

                    INNER JOIN tbl_queued_events_records qe
                        ON qe.patient_enrollment_id = pr.patient_enrollment_id

                    INNER JOIN tbl_sub_strip_event_finding sse
                        ON sse.queued_events_records_id = qe.queued_events_records_id

                    WHERE
                        pr.patient_report_id = 42

                        AND pr.report_status IN ('inprogress', 'completed')

                        AND qe.event_status = 'completed'
                        AND qe.completed_by IS NOT NULL

                        AND sse.is_active = 1
                        AND sse.sub_strip_event_id IS NULL

                        -- QER must overlap this report period
                        AND qe.event_start_datetime < pr.report_due_datetime
                        AND qe.event_end_datetime > pr.report_started_datetime

                        -- Detection must be completed/confirmed
                        AND EXISTS
                        (
                            SELECT 1
                            FROM tbl_detection d
                            WHERE d.queued_events_records_id = sse.queued_events_records_id
                              AND d.sub_strip_event_id IS NULL
                              AND d.is_active = 1
                              AND d.detection_status IN ('submit','confirm-submit')
                        );";


        }
        #endregion
    }



    public class ArrhythmiaResultDTO
    {
        public int af { get; set; }
        public int af_high_hr { get; set; }
        public int svt { get; set; }
        public int pause { get; set; }
        public int heart_block { get; set; }
        public int vt { get; set; }
    }
    public class EventSummaryPdfDTO
    {
        public long total_event_strip_count { get; set; }
        public long patient_event_count { get; set; }
        public long auto_event_count { get; set; }

        #region Query 
        internal static string patient_evnet_count()
        {
            return $@"select count(*) from {Tbl.queued_events_records} where patient_enrollment_id = @patient_enrollment_id";
        }
        #endregion
    }

    public class PatientPdfDTO
    {
        public long patient_report_id { get; set; }
        public long report_assigned_id { get; set; }
        public long patient_enrollment_id { get; set; }

        public long patient_id { get; set; }
        public string? patient_name { get; set; }
        public string? phone_no { get; set; }

        public string? doctor_name { get; set; }
        public string? superviser_name { get; set; }

        public DateTime? report_started_datetime { get; set; }
        public DateTime? report_due_datetime { get; set; }
        public DateTime? created_datetime { get; set; }

        public int duration_in_day { get; set; }



        #region Query 
        internal static string get_patient_details()
        {
            return $@" select a.patient_enrollment_id,a.patient_report_id,a.report_assigned_id,b.created_datetime,b.report_due_datetime,b.report_started_datetime,d.patient_id,d.patient_name,d.phone_no,f.duration_in_day,g.user_name as doctor_name,
                          h.user_name as superviser_name
                          from tbl_report_assigned as a
                          inner join tbl_patient_report as b
                          on a.patient_report_id = b.patient_report_id 
                          inner join tbl_patient_enrollment as c
                          on a.patient_enrollment_id = c.patient_enrollment_id
                          inner join tbl_patient as d
                          on d.patient_id = c.patient_id
                          inner join tbl_test_prices as e
                          on e.test_prices_id = c.estimated_test_prices_id
                          inner join tbl_test_mast as f
                          on f.test_id = e.test_id
                          inner join tbl_user_mast as g
                          on g.user_id = c.referred_physician_id
                          inner join tbl_user_mast as h
                          on h.user_id =b.generate_to
                          where a.assign_status  in ('generated', 'completed') and b.report_status in('inprogress', 'completed') and c.enrollment_status ='completed' and a.patient_report_id = @patient_report_id";
        }

        #endregion

    }
}

