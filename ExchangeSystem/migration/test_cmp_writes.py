"""Sanity tests for cmp_writes.py.

WHY: an audit that matches NOTHING looks exactly like an audit that PASSES. We already shipped one check
(`\\b` in a MariaDB REGEXP) that quietly matched nothing and reported a clean 0 while real bugs sat in the
database. So every detector here is tested against a KNOWN-BAD and a KNOWN-GOOD input before its output is
believed. Run:  python test_cmp_writes.py
"""
import os
import sys

sys.path.insert(0, os.path.dirname(os.path.abspath(__file__)))
import cmp_writes as cw  # noqa: E402

FAILS = []


def check(name, got, want):
    ok = got == want
    print("  %s %-34s got=%s want=%s" % ("ok  " if ok else "FAIL", name, got, want))
    if not ok:
        FAILS.append(name)


print("guarded() — an UPDATE/DELETE that lost its WHERE hits EVERY row")
check("guarded update", cw.guarded("UPDATE t SET a=1 WHERE id=5;"), [True])
check("UNGUARDED update", cw.guarded("UPDATE t SET a=1;"), [False])
check("guarded delete", cw.guarded("DELETE FROM t WHERE id=5;"), [True])
check("UNGUARDED delete", cw.guarded("DELETE FROM t;"), [False])
check("CASE in SET + WHERE",
      cw.guarded("UPDATE t SET a=CASE WHEN x=1 THEN 2 ELSE 3 END WHERE id=5;"), [True])
check("subquery inside WHERE",
      cw.guarded("UPDATE t SET a=1 WHERE id IN (SELECT id FROM u);"), [True])
check("2nd of two is unguarded",
      cw.guarded("UPDATE t SET a=1 WHERE id=5; UPDATE u SET b=2;"), [True, False])
check("T-SQL without semicolons",
      cw.guarded("UPDATE t SET a=1 WHERE id=5 UPDATE u SET b=2"), [True, False])
check("INSERT is not a guarded stmt", cw.guarded("INSERT INTO t (a) VALUES (1);"), [])

print("")
print("inserts() — column list must survive intact and IN ORDER")
check("simple insert",
      cw.inserts("INSERT INTO Tb (A, B, C) VALUES (1,2,3);"), [("tb", ("a", "b", "c"))])
check("schema-qualified + brackets",
      cw.inserts("INSERT INTO [dbo].[Tb] ([A], [B]) VALUES (1,2);"), [("tb", ("a", "b"))])

print("")
print("updates() — target table + SET columns; T-SQL alias resolved to its real table")
check("plain update",
      cw.updates("UPDATE Tb SET A = 1, B = 2 WHERE id=1;"), [("tb", ("a", "b"))])
check("T-SQL alias -> real table",
      cw.updates("UPDATE a SET a.A = 1 FROM Invoices AS a WHERE a.id=1;"), [("invoices", ("a",))])
check("compound assignment +=",
      cw.updates("UPDATE Tb SET Cnt += 1 WHERE id=1;"), [("tb", ("cnt",))])
check("MySQL multi-table form",
      cw.updates("UPDATE Tb, tvp_x AS a SET A = a.A WHERE id=1;"), [("tb", ("a",))])
check("param named IsUpdate is not an UPDATE",
      cw.updates("IF p_IsUpdate = 1 THEN SELECT 1; END IF;"), [])

print("")
print("deletes() — target table")
check("delete from", cw.deletes("DELETE FROM Tb WHERE id=1;"), ["tb"])
check("T-SQL alias delete",
      cw.deletes("DELETE a FROM Tb AS a WHERE a.id=1;"), ["tb"])
check("two deletes, no semicolons",
      cw.deletes("DELETE FROM A WHERE x=1 DELETE FROM B WHERE y=2"), ["a", "b"])

print("")
print("insert_values() — a SWAPPED value still names the right columns and still succeeds")
tsql = "INSERT INTO Led (SafeID, Debit, Credit) VALUES (@SafeID, @Debit, @Credit);"
good = "INSERT INTO Led (SafeID, Debit, Credit) VALUES (p_SafeID, p_Debit, p_Credit);"
swap = "INSERT INTO Led (SafeID, Debit, Credit) VALUES (p_SafeID, p_Credit, p_Debit);"
check("faithful port matches",
      cw.insert_values(tsql) == cw.insert_values(good), True)
check("DEBIT/CREDIT SWAP is caught",
      cw.insert_values(tsql) != cw.insert_values(swap), True)
check("@x vs p_x is not a difference",
      cw.insert_values("INSERT INTO T (A) VALUES (@A);") == cw.insert_values("INSERT INTO T (A) VALUES (p_A);"),
      True)
check("ISNULL vs IFNULL is not a difference",
      cw.insert_values("INSERT INTO T (A) VALUES (ISNULL(@A,0));")
      == cw.insert_values("INSERT INTO T (A) VALUES (IFNULL(p_A,0));"), True)
check("GETDATE vs NOW is not a difference",
      cw.insert_values("INSERT INTO T (A) VALUES (GETDATE());")
      == cw.insert_values("INSERT INTO T (A) VALUES (NOW());"), True)
check("a literal changed IS a difference",
      cw.insert_values("INSERT INTO T (A) VALUES (1);") != cw.insert_values("INSERT INTO T (A) VALUES (0);"),
      True)

print("")
print("update_values() — a wrong VALUE with the right column list")
check("T-SQL 'x += 1' == MySQL 'x = x + 1'",
      cw.update_values("UPDATE users SET Reg='NO', Cnt += 1 WHERE id=@ID;")
      == cw.update_values("UPDATE users SET Reg='NO', Cnt = Cnt + 1 WHERE id=p_ID;"), True)
check("T-SQL FROM clause not swallowed",
      cw.update_values("UPDATE a SET A = b.X FROM T AS a JOIN U AS b ON a.i=b.i WHERE a.i=1;"),
      [("t", ("b.x",))])
check("a SWAPPED value is caught",
      cw.update_values("UPDATE t SET Debit=@D, Credit=@C WHERE i=1;")
      != cw.update_values("UPDATE t SET Debit=p_C, Credit=p_D WHERE i=1;"), True)

print("")
if FAILS:
    print("SANITY FAILED: %s" % ", ".join(FAILS))
    print("The write-path audit CANNOT be trusted until these pass.")
    sys.exit(1)
print("SANITY PASS — every detector fires on known-bad and stays quiet on known-good.")
