-- ======================================================================
-- Name: [adm].[uspAddNew]
-- Desc: Se utiliza para agregar una nueva noticia
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 03/27/2020
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [adm].[uspAddNew]
	@InsertUser		VARCHAR(50),	
	@Title			VARCHAR(30),
	@Description	VARCHAR(200),
	@BannerData		VARBINARY(MAX),
	@BannerExt		VARCHAR(10),
	@InsertDate		DATETIME
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
				INSERT INTO [config].[utbNews] ([Title],[Description],[BannerData],[BannerExt],[InsertDate],[InsertUser],[LastModifyUser])
				VALUES (@Title,@Description,@BannerData,REPLACE(@BannerExt,'.',''),@InsertDate,@InsertUser,@InsertUser)
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