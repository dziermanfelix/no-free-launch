USE NoFreeLaunchDb;
GO

CREATE TABLE dbo.Launches (
    Id NVARCHAR(50) NOT NULL,
    FlightNumber INT NULL,
    Name NVARCHAR(200) NULL,
    DateUtc DATETIME2(7) NULL,
    FetchedAt DATETIME2(7) NOT NULL DEFAULT (GETUTCDATE()),
    CONSTRAINT PK_Launches PRIMARY KEY (Id)
);
GO

CREATE TABLE dbo.Favorites (
    Id INT IDENTITY(1,1) NOT NULL,
    LaunchId NVARCHAR(50) NOT NULL,
    UserId NVARCHAR(128) NOT NULL,
    CreatedAt DATETIME2(7) NOT NULL DEFAULT (GETUTCDATE()),
    CONSTRAINT PK_Favorites PRIMARY KEY (Id),
    CONSTRAINT FK_Favorites_Launches FOREIGN KEY (LaunchId) REFERENCES dbo.Launches(Id) ON DELETE CASCADE,
    CONSTRAINT UQ_Favorites_LaunchId_UserId UNIQUE (LaunchId, UserId)
);
GO

CREATE INDEX IX_Launches_FlightNumber ON dbo.Launches(FlightNumber);
CREATE INDEX IX_Launches_DateUtc ON dbo.Launches(DateUtc);
CREATE INDEX IX_Favorites_LaunchId ON dbo.Favorites(LaunchId);
CREATE INDEX IX_Favorites_UserId ON dbo.Favorites(UserId);
GO