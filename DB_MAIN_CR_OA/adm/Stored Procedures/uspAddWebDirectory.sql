-- ======================================================================
-- Name: [adm].[uspAddWebDirectory]
-- Desc: Se utiliza para agregar un directorio web
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 03/28/2020
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [adm].[uspAddWebDirectory]
	@InsertUser		VARCHAR(50),
	@AppID			INT,
	@Controller		VARCHAR(50),
	@Action			VARCHAR(50),
	@PublicMenu		BIT,
	@AdminMenu		BIT,
	@DisplayName	VARCHAR(50),
	@Order			INT,
	@Parameter		VARCHAR(50) = NULL	
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
				INSERT INTO [adm].[utbWebDirectory] ([AppID],[Controller],[Action],[PublicMenu],[AdminMenu],[DisplayName],[Parameter],[Order],[CreationUser],[LastModifyUser])
				VALUES (@AppID, @Controller, @Action, @PublicMenu, @AdminMenu, @DisplayName, @Parameter, @Order, @InsertUser, @InsertUser)
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
