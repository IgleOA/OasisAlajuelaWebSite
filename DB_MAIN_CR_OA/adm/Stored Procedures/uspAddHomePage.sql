-- ======================================================================
-- Name: [adm].[uspAddHomePage]
-- Desc: Se utiliza para la creación de nuevo HomePage
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 03/27/2020
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [adm].[uspAddHomePage]
	@InsertUser		VARCHAR(50),
	@DVerse			VARCHAR(MAX),
	@DVRef			VARCHAR(50),
	@SVCTitle		VARCHAR(50),
	@SVCDescription	VARCHAR(MAX),
	@SerTitle		VARCHAR(50),
	@SerDescription	VARCHAR(MAX)
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
				UPDATE	[config].[utbHomePage]
				SET		[ActiveFlag] = 0
						,[LastModifyDate] = GETDATE()
						,[LastModifyUser] = @InsertUser
				WHERE 	[ActiveFlag] = 1

				INSERT INTO [config].[utbHomePage] ([DailyVerse],[DailyVerseReference],[ServicesTitle],[ServicesDescription],[SermonsTitle],[SermonsDescription],[InsertUser],[LastModifyUser])
				VALUES (@DVerse, @DVRef, @SVCTitle, @SVCDescription, @SerTitle, @SerDescription, @InsertUser, @InsertUser)
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