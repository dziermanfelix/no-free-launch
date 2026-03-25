USE NoFreeLaunchDb;
GO

IF COL_LENGTH('dbo.Users', 'PasswordHash') IS NULL
BEGIN
    ALTER TABLE dbo.Users ADD PasswordHash NVARCHAR(512) NULL;
END
GO
