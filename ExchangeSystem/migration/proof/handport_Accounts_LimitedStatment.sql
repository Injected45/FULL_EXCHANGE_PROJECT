-- =====================================================================================
-- Hand-port: Accounts_LimitedStatment. Read-only. MySQL/MariaDB 10.4 has no OUTER APPLY (LATERAL). The
-- APPLY was a correlated 'SELECT TOP 1 cb.BName FROM CoBranch WHERE cb.CurrentAccID=a.AccID' referenced
-- exactly once as ISNULL(BranchInfo.BName, a.AccName). Rewritten as an inline correlated scalar subquery
-- (LIMIT 1); a no-match yields NULL, so IFNULL(..) falls back to a.AccName exactly as the OUTER (left)
-- APPLY did. Body is the converter's own output otherwise.
-- =====================================================================================
USE EXCHANGESYS2026;
SET NAMES utf8mb4;
SET collation_connection='utf8mb4_unicode_ci';
SET SESSION sql_mode='IGNORE_SPACE,NO_ENGINE_SUBSTITUTION';

DROP PROCEDURE IF EXISTS `Accounts_LimitedStatment`;
DELIMITER $$
CREATE PROCEDURE `Accounts_LimitedStatment`()
BEGIN
SELECT
Account_GetParentName(a.AccID) AS AccType,

    IFNULL((SELECT cb.BName FROM CoBranch cb WHERE cb.CurrentAccID = a.AccID LIMIT 1), a.AccName) AS AccName,

    a.LimitedVal,
    DATE_FORMAT(LimitedDate, '%Y-%m-%d') AS LimitedDate,
            CASE WHEN EXISTS (SELECT 1 FROM CoBranch cb WHERE cb.CurrentAccID = a.AccID AND cb.BranchType=1)  THEN Branch_GetAccVal(a.BranchID) else  REPLACE(FORMAT(Account_GetAccVal(a.AccID, 1,0), 3), ',', '') END AS AccVal,
        CASE WHEN EXISTS (SELECT 1 FROM CoBranch cb WHERE cb.CurrentAccID = a.AccID AND cb.BranchType=1)  THEN   Branch_GetAccVal(a.BranchID)+a.LimitedVal ELSE  Account_GetAccVal(a.AccID, 1,0) + a.LimitedVal END AS LeftVal

FROM AccountsTb AS a

WHERE a.IsLimited = 1;
END
$$
DELIMITER ;
