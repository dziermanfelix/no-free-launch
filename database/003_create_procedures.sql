USE NoFreeLaunchDb;
GO

CREATE OR ALTER PROCEDURE dbo.AddFavorite
    @LaunchId NVARCHAR(50),
    @UserId INT
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS (SELECT 1 FROM NoFreeLaunchDb.dbo.Favorites WHERE LaunchId = @LaunchId AND UserId = @UserId)
        INSERT INTO NoFreeLaunchDb.dbo.Favorites (LaunchId, UserId)
        VALUES (@LaunchId, @UserId);
END;
GO

CREATE OR ALTER PROCEDURE dbo.RemoveFavorite
    @LaunchId NVARCHAR(50),
    @UserId INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM NoFreeLaunchDb.dbo.Favorites
    WHERE LaunchId = @LaunchId AND UserId = @UserId;
END;
GO
