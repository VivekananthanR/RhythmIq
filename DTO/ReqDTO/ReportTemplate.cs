using elr_API.Common;

namespace elr_API.DTO.ReqDTO
{
    public class ReportTemplate
    {
    }



    public class PatientReportPdfDTO
    {
        public string ecg_base_url { get; set; }

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

                      --AND qer.event_start_datetime >= pe.actual_hookup

                     -- AND qer.event_end_datetime <= DATEADD( HOUR,24,pe.actual_hookup)

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
        #region query 

        internal static string get_pause_data()
        {
            return $@"
                    SELECT
                        -- AM COUNT
                        COUNT(CASE
                            WHEN CAST(pause_data.event_start_datetime AS TIME)
                                 < '12:00:00'
                            THEN 1
                        END) AS am,

                        -- AM LONGEST DURATION
                        ISNULL(
                            MAX(CASE
                                WHEN CAST(pause_data.event_start_datetime AS TIME)
                                     < '12:00:00'
                                THEN pause_data.duration_seconds
                            END),
                            0
                        ) AS am_longest_duration,

                        -- PM COUNT
                        COUNT(CASE
                            WHEN CAST(pause_data.event_start_datetime AS TIME)
                                 >= '12:00:00'
                            THEN 1
                        END) AS pm,

                        -- PM LONGEST DURATION
                        ISNULL(
                            MAX(CASE
                                WHEN CAST(pause_data.event_start_datetime AS TIME)
                                     >= '12:00:00'
                                THEN pause_data.duration_seconds
                            END),
                            0
                        ) AS pm_longest_duration,

                        -- TOTAL COUNT
                        COUNT(*) AS total,

                        -- PAUSE BURDEN
                        ISNULL(
                            CAST(
                                (
                                    CAST(SUM(pause_data.duration_seconds) AS DECIMAL(18,4))
                                    /
                                    NULLIF(
                                        MAX(pause_data.total_report_seconds),
                                        0
                                    )
                                ) * 100
                                AS DECIMAL(10,4)
                            ),
                            0
                        ) AS burden

                    FROM
                    (
                        SELECT DISTINCT
                            qe.queued_events_records_id,
                            qe.event_start_datetime,

                            -- Duration in seconds
                            ISNULL(pause_duration.value1, 0) AS duration_seconds,

                            DATEDIFF(
                                SECOND,
                                pr.report_started_datetime,
                                pr.report_due_datetime
                            ) AS total_report_seconds

                        FROM tbl_patient_report pr

                        INNER JOIN tbl_queued_events_records qe
                            ON qe.patient_enrollment_id =
                               pr.patient_enrollment_id

                        -- PAUSE PARENT FINDING
                        INNER JOIN tbl_sub_strip_event_finding pause_finding
                            ON pause_finding.queued_events_records_id =
                               qe.queued_events_records_id
                           AND pause_finding.event_finding_id = 73
                           AND pause_finding.is_active = 1
                           AND pause_finding.sub_strip_event_id IS NULL

                        -- DYNAMIC DURATION FINDING MAPPING
                        INNER JOIN tbl_event_finding_mast duration_mast
                            ON duration_mast.parent_finding_id =
                               pause_finding.event_finding_id
                           AND duration_mast.is_active = 1

                        -- ACTUAL DURATION VALUE
                        INNER JOIN tbl_sub_strip_event_finding pause_duration
                            ON pause_duration.queued_events_records_id =
                               qe.queued_events_records_id
                           AND pause_duration.event_finding_id =
                               duration_mast.event_finding_id
                           AND pause_duration.is_active = 1
                           AND pause_duration.sub_strip_event_id IS NULL

                        WHERE pr.patient_report_id = @patient_report_id
                          AND pr.report_status IN ('inprogress', 'completed')
                          AND qe.is_active = 1
                          AND qe.event_status = 'completed'
                          AND qe.completed_by IS NOT NULL

                          -- REPORT WINDOW
                          AND qe.event_start_datetime >=
                              pr.report_started_datetime
                          AND qe.event_start_datetime <
                              pr.report_due_datetime

                          -- COMPLETED PARENT DETECTION
                          AND EXISTS
                          (
                              SELECT 1
                              FROM tbl_detection d
                              WHERE d.queued_events_records_id =
                                    qe.queued_events_records_id
                                AND d.sub_strip_event_id IS NULL
                                AND d.is_active = 1
                                AND d.detection_status IN
                                    ('submit', 'confirm-submit')
                          )
                    ) pause_data;
                ";
        }

        #endregion


        //internal static string get_pause_data()
        //{
        //    return $@"
        //        SELECT

        //            -- =========================================
        //            -- AM COUNT
        //            -- =========================================
        //            COUNT(CASE
        //                WHEN CAST(pause_data.event_start_datetime AS TIME)
        //                     < '12:00:00'
        //                THEN 1
        //            END) AS am,

        //            -- =========================================
        //            -- AM LONGEST DURATION
        //            -- Finding 78 value1 = seconds
        //            -- =========================================
        //            ISNULL(
        //                MAX(CASE
        //                    WHEN CAST(
        //                        pause_data.event_start_datetime AS TIME
        //                    ) < '12:00:00'
        //                    THEN pause_data.duration_seconds
        //                END),
        //                0
        //            ) AS am_longest_duration,

        //            -- =========================================
        //            -- PM COUNT
        //            -- =========================================
        //            COUNT(CASE
        //                WHEN CAST(pause_data.event_start_datetime AS TIME)
        //                     >= '12:00:00'
        //                THEN 1
        //            END) AS pm,

        //            -- =========================================
        //            -- PM LONGEST DURATION
        //            -- =========================================
        //            ISNULL(
        //                MAX(CASE
        //                    WHEN CAST(
        //                        pause_data.event_start_datetime AS TIME
        //                    ) >= '12:00:00'
        //                    THEN pause_data.duration_seconds
        //                END),
        //                0
        //            ) AS pm_longest_duration,

        //            -- =========================================
        //            -- TOTAL COUNT
        //            -- =========================================
        //            COUNT(*) AS total,

        //            -- =========================================
        //            -- TOTAL LONGEST DURATION
        //            -- =========================================
        //            ISNULL(
        //                MAX(pause_data.duration_seconds),
        //                0
        //            ) AS longest_duration,

        //            -- =========================================
        //            -- PAUSE BURDEN (Updated logic using value1)
        //            --
        //            -- SUM OF PAUSE SECONDS (value1)
        //            -- /
        //            -- REPORT DURATION SECONDS
        //            -- * 100
        //            -- =========================================
        //            ISNULL(
        //                CAST(
        //                    (
        //                        CAST(
        //                            SUM(pause_data.duration_seconds) AS DECIMAL(18,4)
        //                        )
        //                        /
        //                        NULLIF(
        //                            DATEDIFF(
        //                                SECOND,
        //                                MAX(pause_data.report_started_datetime),
        //                                MAX(pause_data.report_due_datetime)
        //                            ),
        //                            0
        //                        )
        //                    ) * 100
        //                    AS DECIMAL(10,4)
        //                ),
        //                0
        //            ) AS burden

        //        FROM
        //        (
        //            SELECT DISTINCT

        //                -- =====================================
        //                -- ONE QER = ONE PAUSE FILE
        //                -- =====================================
        //                qe.queued_events_records_id,
        //                qe.event_start_datetime,
        //                qe.event_end_datetime,

        //                -- =====================================
        //                -- PAUSE DURATION
        //                --
        //                -- 73 = Pause
        //                -- 78 = Duration
        //                -- value1 = seconds
        //                -- =====================================
        //                ISNULL(
        //                    pause_duration.value1,
        //                    0
        //                ) AS duration_seconds,

        //                pr.report_started_datetime,
        //                pr.report_due_datetime

        //            FROM tbl_patient_report pr

        //            INNER JOIN tbl_queued_events_records qe
        //                ON qe.patient_enrollment_id =
        //                   pr.patient_enrollment_id

        //            -- =========================================
        //            -- PAUSE PARENT FINDING
        //            -- 73 = Pause
        //            -- =========================================
        //            INNER JOIN tbl_sub_strip_event_finding pause_finding
        //                ON pause_finding.queued_events_records_id =
        //                   qe.queued_events_records_id
        //                AND pause_finding.event_finding_id = 73
        //                AND pause_finding.is_active = 1
        //                AND pause_finding.sub_strip_event_id IS NULL

        //            -- =========================================
        //            -- PAUSE DURATION CHILD
        //            -- 78 = [xx.x] seconds Pause
        //            -- =========================================
        //            INNER JOIN tbl_event_finding_mast duration_mast
        //                ON duration_mast.parent_finding_id =
        //                   pause_finding.event_finding_id
        //                AND duration_mast.event_finding_id = 78
        //                AND duration_mast.is_active = 1

        //            -- =========================================
        //            -- ACTUAL DURATION VALUE
        //            -- =========================================
        //            INNER JOIN tbl_sub_strip_event_finding pause_duration
        //                ON pause_duration.queued_events_records_id =
        //                   qe.queued_events_records_id
        //                AND pause_duration.event_finding_id =
        //                    duration_mast.event_finding_id
        //                AND pause_duration.is_active = 1
        //                AND pause_duration.sub_strip_event_id IS NULL

        //            WHERE
        //                pr.patient_report_id =
        //                    @patient_report_id
        //                AND pr.report_status IN
        //                (
        //                    'inprogress',
        //                    'completed'
        //                )
        //                AND qe.event_status =
        //                    'completed'
        //                AND qe.completed_by IS NOT NULL

        //                -- =====================================
        //                -- 24 HOUR REPORT WINDOW
        //                -- =====================================
        //                AND qe.event_start_datetime >=
        //                    pr.report_started_datetime
        //                AND qe.event_start_datetime <
        //                    pr.report_due_datetime

        //                -- =====================================
        //                -- DETECTION COMPLETED
        //                -- =====================================
        //                AND EXISTS
        //                (
        //                    SELECT 1
        //                    FROM tbl_detection d
        //                    WHERE
        //                        d.queued_events_records_id =
        //                            qe.queued_events_records_id
        //                        AND d.sub_strip_event_id IS NULL
        //                        AND d.is_active = 1
        //                        AND d.detection_status IN
        //                        (
        //                            'submit',
        //                            'confirm-submit'
        //                        )
        //                )

        //        ) pause_data;

        //        ";
        //}
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

        public decimal longest_af_value1 { get; set; }

        public decimal longest_af_value2 { get; set; }

        #region Query

        internal static string get_af_burden_data()
        {
            return $@"
        SELECT
            -- AF EPISODE COUNT
            (
                SELECT COUNT(DISTINCT d.queued_events_records_id)
                FROM tbl_patient_report pr_ep
                INNER JOIN tbl_queued_events_records qe_ep
                    ON qe_ep.patient_enrollment_id =
                       pr_ep.patient_enrollment_id
                INNER JOIN tbl_detection d
                    ON d.queued_events_records_id =
                       qe_ep.queued_events_records_id
                   AND d.is_active = 1
                   AND d.parent_detection_id IS NULL
                INNER JOIN tbl_sub_strip_event_finding ssf
                    ON ssf.detection_id = d.detection_id
                   AND ssf.is_active = 1
                   AND ssf.event_finding_id IN (35, 36, 37)
                WHERE pr_ep.patient_report_id = @patient_report_id
            ) AS episode_count,

            -- HIGHEST HR
            ISNULL(
                (
                    SELECT MAX(qe.bpm)
                    FROM tbl_patient_report pr_hr
                    INNER JOIN tbl_queued_events_records qe
                        ON qe.patient_enrollment_id =
                           pr_hr.patient_enrollment_id
                    INNER JOIN tbl_detection d
                        ON d.queued_events_records_id =
                           qe.queued_events_records_id
                       AND d.is_active = 1
                       AND d.parent_detection_id IS NULL
                    INNER JOIN tbl_sub_strip_event_finding ssf
                        ON ssf.detection_id = d.detection_id
                       AND ssf.is_active = 1
                       AND ssf.event_finding_id IN (35, 36, 37)
                    WHERE pr_hr.patient_report_id = @patient_report_id
                ),
                0
            ) AS high_hr,

            -- LONGEST AF
            ISNULL(longest_af.value1, 0) AS longest_af_value1,
            ISNULL(longest_af.value2, 0) AS longest_af_value2,

            -- AF BURDEN
            ISNULL(burden_data.total_af_percentage, 0) AS burden

        FROM tbl_patient_report pr

        OUTER APPLY
        (
            SELECT TOP 1
                duration.value1,
                duration.value2
            FROM tbl_queued_events_records qe
            INNER JOIN tbl_detection d
                ON d.queued_events_records_id =
                   qe.queued_events_records_id
               AND d.is_active = 1
               AND d.detection_status = 'submit'
               AND d.parent_detection_id IS NULL
               AND d.sub_strip_event_id IS NULL

            INNER JOIN tbl_sub_strip_event_finding parent_finding
                ON parent_finding.detection_id = d.detection_id
               AND parent_finding.is_active = 1
               AND parent_finding.event_finding_id IN (35, 36, 37)

            INNER JOIN tbl_event_finding_mast duration_mast
                ON duration_mast.parent_finding_id =
                   parent_finding.event_finding_id
               AND duration_mast.is_active = 1

            INNER JOIN tbl_sub_strip_event_finding duration
                ON duration.detection_id = d.detection_id
               AND duration.event_finding_id =
                   duration_mast.event_finding_id
               AND duration.is_active = 1

            WHERE qe.patient_enrollment_id = pr.patient_enrollment_id
              AND qe.is_active = 1

            ORDER BY
                (
                    ISNULL(duration.value1, 0) * 60
                    + ISNULL(duration.value2, 0)
                ) DESC
        ) longest_af

       OUTER APPLY
                (
                    SELECT
                        SUM(
                            CAST(
                                (
                                    CAST(f.value1 AS DECIMAL(18,4)) * 60
                                    + CAST(f.value2 AS DECIMAL(18,4))
                                )
                                / NULLIF(r.total_report_seconds, 0)
                                * 100
                                AS DECIMAL(10,4)
                            )
                        ) AS total_af_percentage
                    FROM tbl_sub_strip_event_finding f
                    INNER JOIN tbl_queued_events_records a
                        ON a.queued_events_records_id = f.queued_events_records_id
                    INNER JOIN tbl_patient_enrollment b
                        ON b.patient_enrollment_id = a.patient_enrollment_id
                    INNER JOIN tbl_patient_report c
                        ON c.patient_enrollment_id = b.patient_enrollment_id
                    INNER JOIN tbl_detection d
                        ON d.detection_id = f.detection_id
                       AND d.is_active = 1
                       AND d.parent_detection_id IS NULL
                    INNER JOIN tbl_event_finding_mast duration_mast
                        ON duration_mast.event_finding_id = f.event_finding_id
                       AND duration_mast.parent_finding_id IN (35, 36, 37)
                       AND duration_mast.is_active = 1
                    CROSS APPLY
                    (
                        SELECT
                            DATEDIFF(
                                SECOND,
                                MAX(c2.report_started_datetime),
                                MAX(c2.report_due_datetime)
                            ) AS total_report_seconds
                        FROM tbl_patient_report c2
                        WHERE c2.patient_enrollment_id = b.patient_enrollment_id
                    ) r
                    WHERE c.patient_report_id = @patient_report_id
                      AND f.sub_strip_event_id IS NULL
                      AND f.is_active = 1
                ) burden_data

        WHERE pr.patient_report_id = @patient_report_id; ";
        }

        #endregion
        //        #region Query
        //        internal static string get_af_burden_data()
        //        {
        //            return $@"
        //                    SELECT
        //                    -- AF EPISODE COUNT
        //                    (
        //                        SELECT
        //                            COUNT(DISTINCT sse.sub_strip_event_id)
        //                        FROM tbl_patient_report pr_ep
        //                        INNER JOIN tbl_queued_events_records qe_ep
        //                            ON qe_ep.patient_enrollment_id =
        //                               pr_ep.patient_enrollment_id
        //                        INNER JOIN tbl_sub_strip_event sse
        //                            ON sse.queued_events_records_id =
        //                               qe_ep.queued_events_records_id
        //                            AND sse.is_active = 1
        //                        INNER JOIN tbl_sub_strip_event_finding ssf
        //                            ON ssf.sub_strip_event_id =
        //                               sse.sub_strip_event_id
        //                            AND ssf.is_active = 1
        //                            AND ssf.event_finding_id IN
        //                            (
        //                                35,
        //                                36,
        //                                37
        //                            )
        //                        WHERE
        //                            pr_ep.patient_report_id =
        //                                @patient_report_id
        //                    ) AS episode_count,

        //    -- =========================================
        //    -- HIGHEST HR
        //    -- =========================================
        //    ISNULL(
        //        (
        //            SELECT MAX(qe.bpm)
        //            FROM tbl_patient_report pr_hr
        //            INNER JOIN tbl_queued_events_records qe
        //                ON qe.patient_enrollment_id =
        //                   pr_hr.patient_enrollment_id
        //            INNER JOIN tbl_sub_strip_event sse
        //                ON sse.queued_events_records_id =
        //                   qe.queued_events_records_id
        //                AND sse.is_active = 1
        //            INNER JOIN tbl_sub_strip_event_finding ssf
        //                ON ssf.sub_strip_event_id =
        //                   sse.sub_strip_event_id
        //                AND ssf.is_active = 1
        //                AND ssf.event_finding_id IN
        //                (
        //                    35,
        //                    36,
        //                    37
        //                )
        //            WHERE
        //                pr_hr.patient_report_id =
        //                    @patient_report_id
        //        ),
        //        0
        //    ) AS high_hr,

        //    -- =========================================
        //    -- LONGEST AF
        //    -- =========================================
        //    ISNULL(
        //        longest_af.value1,
        //        0
        //    ) AS longest_af_value1,

        //    ISNULL(
        //        longest_af.value2,
        //        0
        //    ) AS longest_af_value2,

        //    -- =========================================
        //    -- AF BURDEN (Summed into a single value)
        //    -- =========================================
        //    ISNULL(burden_data.total_pause_percentage, 0) AS burden,

        //    -- =========================================
        //    -- LONGEST AF DURATION
        //    -- =========================================
        //    ISNULL(longest_af.value1, 0) AS longest_value1,
        //    ISNULL(longest_af.value2, 0) AS longest_value2

        //FROM(
        //    SELECT 1 AS dummy
        //) x

        //-- =============================================
        //-- LONGEST AF
        //-- =============================================
        //OUTER APPLY(
        //    SELECT TOP 1
        //        af_duration.value1,
        //        af_duration.value2
        //    FROM tbl_patient_report pr
        //    INNER JOIN tbl_queued_events_records qe
        //        ON qe.patient_enrollment_id = 
        //           pr.patient_enrollment_id
        //    INNER JOIN tbl_sub_strip_event_finding af_finding
        //        ON af_finding.queued_events_records_id = 
        //           qe.queued_events_records_id
        //    INNER JOIN tbl_event_finding_mast duration_mast
        //        ON duration_mast.parent_finding_id =
        //           af_finding.event_finding_id
        //        AND duration_mast.is_active = 1
        //    INNER JOIN tbl_sub_strip_event_finding af_duration
        //        ON af_duration.queued_events_records_id =
        //           af_finding.queued_events_records_id
        //        AND af_duration.event_finding_id =
        //             duration_mast.event_finding_id
        //        AND af_duration.is_active = 1
        //        AND af_duration.sub_strip_event_id IS NULL
        //    WHERE
        //        pr.patient_report_id = @patient_report_id
        //        AND af_finding.event_finding_id IN
        //        (
        //            35,
        //            36,
        //            37
        //        )
        //        AND af_finding.is_active = 1
        //        AND af_finding.sub_strip_event_id IS NOT NULL
        //    ORDER BY
        //        (
        //            ISNULL(
        //                af_duration.value1,
        //                0
        //            ) * 60
        //            +
        //            ISNULL(
        //                af_duration.value2,
        //                0
        //            )
        //        ) DESC
        //) longest_af

        //-- =============================================
        //-- NEW AF BURDEN / PAUSE PERCENTAGE (Aggregated to 1 Row)
        //-- =============================================
        //OUTER APPLY (
        //    SELECT
        //        -- Summing the duration percentages across all matching rows
        //        SUM(
        //            CAST(
        //                (
        //                    CAST(f.value1 AS DECIMAL(18,4)) * 60
        //                    + CAST(f.value2 AS DECIMAL(18,4))
        //                )
        //                / NULLIF(r.total_report_seconds, 0)
        //                * 100
        //                AS DECIMAL(10,4)
        //            )
        //        ) AS total_pause_percentage
        //    FROM tbl_sub_strip_event_finding f
        //    INNER JOIN tbl_queued_events_records a
        //        ON a.queued_events_records_id = f.queued_events_records_id
        //    INNER JOIN tbl_patient_enrollment b
        //        ON b.patient_enrollment_id = a.patient_enrollment_id
        //    INNER JOIN tbl_patient_report c
        //        ON c.patient_enrollment_id = b.patient_enrollment_id
        //    CROSS APPLY(
        //        SELECT
        //            DATEDIFF(
        //                SECOND,
        //                MAX(c2.report_started_datetime),
        //                MAX(c2.report_due_datetime)
        //            ) AS total_report_seconds
        //        FROM tbl_patient_report c2
        //        WHERE c2.patient_enrollment_id = b.patient_enrollment_id
        //    ) r
        //    WHERE c.patient_report_id = @patient_report_id
        //      AND f.sub_strip_event_id IS NULL
        //      AND f.is_active = 1
        //) burden_data;
        //                        ";
        //        }
        //        #endregion
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
                            WHEN sse.event_finding_id IN (35, 36, 37)
                            THEN 1
                        END) AS af,

                        COUNT(CASE
                            WHEN sse.event_finding_id = 34
                            THEN 1
                        END) AS svt,

                        COUNT(CASE
                            WHEN sse.event_finding_id = 73
                            THEN 1
                        END) AS pause,

                        COUNT(CASE
                            WHEN sse.event_finding_id IN (57, 58, 59, 60, 61, 62, 63)
                            THEN 1
                        END) AS heart_block,

                        COUNT(CASE
                            WHEN sse.event_finding_id = 48
                            THEN 1
                        END) AS vt
                    FROM tbl_sub_strip_event_finding AS sse
                    INNER JOIN tbl_queued_events_records AS b
                        ON sse.queued_events_records_id = b.queued_events_records_id
                    INNER JOIN tbl_patient_enrollment AS c
                        ON c.patient_enrollment_id = b.patient_enrollment_id
                    INNER JOIN tbl_detection AS d
                        ON d.detection_id = sse.detection_id
                    WHERE c.patient_enrollment_id = @patient_enrollment_id
                      AND sse.is_active = 1
                      AND d.sub_strip_event_id IS NULL;";


        }
        #endregion
    }



    public class ArrhythmiaResultDTO
    {
        public int af { get; set; }
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
            return $@"select count(*) from {Tbl.queued_events_records} where patient_enrollment_id = @patient_enrollment_id and is_active = 1 and completed_by is not null and completed_datetime is not null";
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

        public DateTime? report_generated_datetime { get; set; }
        public DateTime? modified_datetime { get; set; }
        public DateTime? report_started_datetime { get; set; }
        public DateTime? report_due_datetime { get; set; }
        public DateTime? created_datetime { get; set; }

        public int duration_in_day { get; set; }



        #region Query 
        internal static string get_patient_details()
        {
            return $@" select a.patient_enrollment_id,a.patient_report_id,a.report_assigned_id,b.modified_datetime,b.report_generated_datetime,b.created_datetime,b.report_due_datetime,b.report_started_datetime,d.patient_id,d.patient_name,d.phone_no,f.duration_in_day,g.user_name as doctor_name,
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
                          where a.assign_status  in ('generated', 'completed') and b.report_status in('inprogress', 'completed')  and a.patient_report_id = @patient_report_id";
        }

        #endregion

    }
}

