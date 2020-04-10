-- ======================================================================
-- Name: [adm].[uspAddSermon]
-- Desc: Se utiliza para agregar una sermon
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 03/27/2020
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [adm].[uspAddSermon]
	@InsertUser		VARCHAR(50),	
	@Title			VARCHAR(30),
	@Description	VARCHAR(MAX),
	@Tags			VARCHAR(MAX),
	@MinisterID		INT,
	@BannerData		VARBINARY(MAX),
	@BannerExt		VARCHAR(10),
	@SermonDate		DATETIME,
	@SermonURL		VARCHAR(500)
AS 
    BEGIN
        SET NOCOUNT ON
        SET XACT_ABORT ON
                           
        BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT
            DECLARE @lLocalTran BIT = 0
                               
            IF @@TRANCOUNT = 0 
                BEGIN
                    BEGIN TRANSACTION
                    SET @lLocalTran = 1
                END

            -- =======================================================
				INSERT INTO [config].[utbSermons] ([Title],[Description],[Tags],[MinisterID],[SermonDate],[SermonURL],[BackgroundImage],[BackgroundExt],[InsertUser],[LastModifyUser])
				VALUES (@Title,@Description,@Tags,@MinisterID,@SermonDate,@SermonURL,@BannerData,REPLACE(@BannerExt,'.',''),@InsertUser,@InsertUser)
			-- =======================================================

        IF ( @@trancount > 0
                 AND @lLocalTran = 1
               ) 
                BEGIN
                    COMMIT TRANSACTION
                END
        END TRY
        BEGIN CATCH
            IF ( @@trancount > 0
                 AND XACT_STATE() <> 0
               ) 
                BEGIN
                    ROLLBACK TRANSACTION
                END

            SELECT  @lErrorMessage = ERROR_MESSAGE() ,
                    @lErrorSeverity = ERROR_SEVERITY() ,
                    @lErrorState = ERROR_STATE()       

            RAISERROR (@lErrorMessage, @lErrorSeverity, @lErrorState);
        END CATCH
    END

    SET NOCOUNT OFF
    SET XACT_ABORT OFF
