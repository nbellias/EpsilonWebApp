USE [epsilondb]
GO

-- Clear old test data to start fresh
TRUNCATE TABLE [dbo].[Customers];

WITH 
-- 1. Expanded Greek Name Pool (20 combinations)
GreekNames AS (
    SELECT * FROM (VALUES 
    (N'Γιάννης'), (N'Μαρία'), (N'Κώστας'), (N'Ελένη'), (N'Δημήτρης'),
    (N'Άννα'), (N'Νίκος'), (N'Σοφία'), (N'Μιχάλης'), (N'Κατερίνα')) AS FN(First)
    CROSS JOIN 
    (VALUES (N'Παπαδόπουλος'), (N'Γεωργίου'), (N'Δημητρίου'), (N'Βασιλείου')) AS LN(Last)
),
-- 2. Expanded Latin Name Pool (20 combinations)
LatinNames AS (
    SELECT * FROM (VALUES 
    ('James'), ('Emma'), ('Robert'), ('Olivia'), ('William'),
    ('Sophia'), ('Michael'), ('Isabella'), ('David'), ('Mia')) AS FN(First)
    CROSS JOIN 
    (VALUES ('Smith'), ('Johnson'), ('Williams'), ('Brown')) AS LN(Last)
),
-- 3. Combine them into a single indexed list
CombinedNames AS (
    SELECT ROW_NUMBER() OVER(ORDER BY (SELECT NULL)) as ID, First + N' ' + Last as FullName
    FROM (
        SELECT * FROM GreekNames -- First 40 entries
        UNION ALL
        SELECT * FROM LatinNames -- Next 40 entries
        -- Add more variants if you need to reach exactly 100 unique names
        UNION ALL 
        SELECT N'Αλέξανδρος', N'Νικολάου' UNION ALL SELECT N'Χριστίνα', N'Αγγελίδη'
        UNION ALL SELECT 'Lucas', 'Davies' UNION ALL SELECT 'Chloe', 'Evans'
    ) AS AllNames
),
-- 4. The Loop
Generator AS (
    SELECT 1 AS n
    UNION ALL
    SELECT n + 1 FROM Generator WHERE n < 100
)
INSERT INTO [dbo].[Customers] (Id, CompanyName, ContactName, Address, City, Region, PostalCode, Country, Phone)
SELECT 
    NEWID(),
    -- Unique Companies
    CASE 
        WHEN n <= 50 THEN N'Ελληνική Επιχείρηση ' + CAST(n AS NVARCHAR(5))
        ELSE 'International Corp ' + CAST(n AS NVARCHAR(5))
    END,
    -- Unique Names (Picking from our combined list)
    ISNULL((SELECT FullName FROM CombinedNames WHERE ID = n), 'Backup Name ' + CAST(n AS NVARCHAR(5))),
    -- Unique Addresses
    CHOOSE((n % 5) + 1, N'Οδός Ερμού ', N'Λεωφ. Συγγρού ', 'High Street ', 'Broadway ', 'Main Ave ') + CAST(n * 2 AS NVARCHAR(10)),
    -- Cities
    CASE WHEN n <= 50 THEN CHOOSE((n % 3) + 1, N'Αθήνα', N'Θεσσαλονίκη', N'Πάτρα') 
         ELSE CHOOSE((n % 3) + 1, 'London', 'New York', 'Berlin') END,
    -- Regions
    CASE WHEN n <= 50 THEN CHOOSE((n % 3) + 1, N'Αττική', N'Μακεδονία', N'Αχαΐα') 
         ELSE CHOOSE((n % 3) + 1, 'UK', 'NY', 'BE') END,
    -- Postal Code
    CAST(10000 + n AS NVARCHAR(10)),
    -- Country
    CASE WHEN n <= 50 THEN N'Ελλάδα' ELSE 'International' END,
    -- Phone
    '+' + CAST(n * 1111111 AS NVARCHAR(20))
FROM Generator
OPTION (MAXRECURSION 100);
