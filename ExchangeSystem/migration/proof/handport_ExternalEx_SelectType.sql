-- =====================================================================================
-- Hand-port: ExternalEx_SelectType. Read-only. MySQL/MariaDB has no FULL OUTER JOIN. The single
-- FULL OUTER JOIN (ExternalEx a <-> CoBranch c ON a.BranchDeleviredID=c.ID) is followed by WHERE
-- a.IsActive=1, which eliminates every c-only (right) row (their a.IsActive is NULL <> 1). With the
-- right-only side gone, FULL OUTER JOIN is IDENTICAL to LEFT JOIN here -> downgraded. Body is the
-- converter's own output, unchanged except FULL OUTER JOIN -> LEFT JOIN.
-- =====================================================================================
USE EXCHANGESYS2026;
SET NAMES utf8mb4;
SET collation_connection='utf8mb4_unicode_ci';
SET SESSION sql_mode='IGNORE_SPACE,NO_ENGINE_SUBSTITUTION';

DROP PROCEDURE IF EXISTS `ExternalEx_SelectType`;
DELIMITER $$
CREATE PROCEDURE `ExternalEx_SelectType`(IN `p_BranchID` INT, IN `p_SelectType` INT)
BEGIN
DECLARE v_IsInOrOut TINYINT;
DECLARE v_IsMain BIT;
DECLARE v_currD DATE DEFAULT NOW();
START TRANSACTION;


  SELECT a.IsMain INTO v_IsMain FROM
          CountiresTb AS a
      INNER JOIN
        ExternalEx AS b
          ON a.ID = b.CountryIDFrom
  WHERE
          b.RecievedBranchID = p_BranchID;
  IF v_IsMain = 1
     THEN
SET v_IsInOrOut = 0;
    END IF;
  IF (v_IsMain = 0
    OR
    v_IsMain IS NULL)
     THEN
SET v_IsInOrOut = 1;
    END IF;


  IF p_SelectType = 1
     THEN
      SELECT
              a.Code            AS 'الرمز',
              a.InsertDate      AS 'التاريخ',
              a.SenderName      AS 'الراسل',
              a.RecievedName    AS 'المستلم',
              a.CurrRecievedVal AS 'قيمة الحوالة',
              a.ExVal           AS 'العمولة',
              b.BName           AS 'من',
              CASE                        WHEN a.BranchDeleviredID > 0                          THEN                          c.BName                        ELSE                        'خارج المنظومة'                END               AS 'إلى'
      FROM
              ExternalEx AS a
          INNER JOIN
            CoBranch AS b
              ON a.RecievedBranchID = b.ID
          LEFT JOIN
            CoBranch AS c
              ON a.BranchDeleviredID = c.ID
      WHERE
              a.IsActive = 1
              AND a.IsCanceled = 0
              AND a.IsDelivered = 0
              AND a.IsConfirmed = 0
              AND a.RecievedBranchID = p_BranchID;
    END IF;


  IF p_SelectType = 4
     THEN
      SELECT
              x.Code         AS 'الرمز',
              x.InsertDate   AS 'التاريخ',
              x.SenderName   AS 'الراسل',
              x.RecievedName AS 'المستلم',
              x.NetTotal     AS 'قيمة الحوالة',
              x.ExVal        AS 'العمولة',
              x.BName        AS 'من',
              x.CaseStauts   AS 'إلى',
              x.SendStatus   AS 'الحالة',
              x.RecievedBranchID
      FROM
              (   SELECT
                          a.Code,
                          a.InsertDate,
                          a.SenderName,
                          a.RecievedName,
                          a.NetTotal,
                          a.ExVal,
                          b.BName,
                          CaseStauts = CASE                                                WHEN a.BranchDeleviredID > 0                                                  THEN                                                  c.BName                                                ELSE                                                'خارج المنظومة'                                        END,
                          SendStatus = CASE                                                WHEN a.IsCanceled = 0                                                  AND a.IsConfirmed = 1                                                  THEN                                                  'جاهزة للتسليم'                                        END,
                          a.RecievedBranchID
                  FROM
                          ExternalEx AS a
                      INNER JOIN
                        CoBranch AS b
                            ON a.RecievedBranchID = b.ID
                      INNER JOIN
                        CoBranch AS c
                            ON a.BranchDeleviredID = c.ID
                  WHERE
                          a.IsCanceled = 0
                          AND IsConfirmed = 1
                          AND a.IsDelivered = 0
                          AND a.BranchDeleviredID = p_BranchID
                  UNION
                  SELECT
                          a.Code,
                          a.InsertDate,
                          a.SenderName,
                          a.RecievedName,
                          a.NetTotal,
                          a.ExVal,
                          b.AccName AS BName,
                          CaseStauts = CASE                                                WHEN a.BranchDeleviredID > 0                                                  THEN                                                  c.BName                                                ELSE                                                'خارج المنظومة'                                        END,
                          SendStatus = CASE                                                WHEN a.IsCanceled = 0                                                  AND a.IsConfirmed = 1                                                  THEN                                                  'جاهزة للتسليم'                                        END,
                          a.RecievedBranchID
                  FROM
                          ExternalEx AS a
                      INNER JOIN
                        AccountsTb AS b
                            ON a.RecievedBranchID = b.AccID
                      INNER JOIN
                        CoBranch AS c
                            ON a.BranchDeleviredID = c.ID
                  WHERE
                          a.IsCanceled = 0
                          AND IsConfirmed = 1
                          AND a.IsDelivered = 0
                          AND b.AccParent LIKE '101012%'
                          AND a.BranchDeleviredID = p_BranchID) AS x;
    END IF;
  IF p_SelectType = 5
     THEN
      SELECT
              x.Code         AS 'الرمز',
              x.InsertDate   AS 'التاريخ',
              x.SenderName   AS 'الراسل',
              x.RecievedName AS 'المستلم',
              x.NetTotal     AS 'قيمة الحوالة',
              x.ExVal        AS 'العمولة',
              x.BName        AS 'من',
              x.CaseStauts   AS 'إلى',
              x.SendStatus   AS 'الحالة',
              x.RecievedBranchID
      FROM
              (   SELECT
                          a.Code,
                          a.InsertDate,
                          a.SenderName,
                          a.RecievedName,
                          a.CurrDeliveredVal AS NetTotal,
                          a.ExVal,
                          b.BName,
                          CaseStauts = CASE                                                WHEN (a.BranchDeleviredID = p_BranchID)                                                  THEN                                                  c.BName                                                ELSE                                                'خارج المنظومة'                                        END,
                          SendStatus = CASE                                                WHEN a.IsCanceled = 2                                                  AND a.IsConfirmed = 0                                                  THEN                                                  'ملغاة'                                                WHEN                                                  a.IsCanceled = 2                                                  AND a.IsConfirmed = 1                                                  THEN                                                  'معتمدة ملغاة'                                        END,
                          a.RecievedBranchID
                  FROM
                          ExternalEx AS a
                      INNER JOIN
                        CoBranch AS b
                            ON a.RecievedBranchID = b.ID
                      LEFT JOIN
                        CoBranch AS c
                            ON a.BranchDeleviredID = c.ID
                  WHERE
                          a.IsCanceled = 2
                          AND (   a.IsConfirmed = 0
                                  OR (    a.IsConfirmed = 1
                                          AND a.IsInOrOut = 0))
                          AND a.IsDelivered = 0
                          AND (   (       a.BranchDeleviredID = p_BranchID
                                          AND a.IsInOrOut = 1)
                                  OR (    a.RecievedBranchID = p_BranchID
                                          AND a.IsInOrOut = 0))
                  UNION
                  SELECT
                          a.Code,
                          a.InsertDate,
                          a.SenderName,
                          a.RecievedName,
                          a.CurrDeliveredVal AS NetTotal,
                          a.ExVal,
                          c.BName,
                          CaseStauts = CASE                                                WHEN (a.OurAccID <> 0                                                    OR a.OurAccID <> -1)                                                  THEN                                                  b.AccName                                                WHEN (a.OurAccID = 0                                                    OR a.OurAccID = -1)                                                  THEN                                                  'خارج المنظومة'                                        END,
                          SendStatus = CASE                                                WHEN a.IsCanceled = 2                                                  AND a.IsConfirmed = 0                                                  THEN                                                  'ملغاة'                                                WHEN                                                  a.IsCanceled = 2                                                  AND a.IsConfirmed = 1                                                  THEN                                                  'معتمدة ملغاة'                                        END,
                          a.RecievedBranchID
                  FROM
                          ExternalEx AS a
                      INNER JOIN
                        AccountsTb AS b
                            ON a.OurAccID = b.AccID
                      INNER JOIN
                        CoBranch AS c
                            ON a.RecievedBranchID = c.ID
                  WHERE
                          a.IsCanceled = 2
                          AND (   IsConfirmed = 0
                                  OR (    a.IsConfirmed = 1
                                          AND a.IsInOrOut = 0))
                          AND a.IsDelivered = 0
                          AND b.AccParent LIKE '101012%'
                          AND (   (       (       a.BranchDeleviredID = 0
                                                  OR a.BranchDeleviredID = -1
                                                  OR a.BranchDeleviredID = p_BranchID)
                                          AND a.IsInOrOut = 1)
                                  OR (    a.RecievedBranchID = p_BranchID
                                          AND a.IsInOrOut = 0))) AS x;
    END IF;



  COMMIT;
END
$$
DELIMITER ;
