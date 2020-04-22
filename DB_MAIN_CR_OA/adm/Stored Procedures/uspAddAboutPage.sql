-- ======================================================================
-- Name: [adm].[uspAddAboutPage]
-- Desc: Se utiliza para la creación de nuevo About Page
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 03/27/2020
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [adm].[uspAddAboutPage]
	@InsertUser		VARCHAR(50),
	@History		VARCHAR(MAX),
	@Mision			VARCHAR(MAX),
	@Vision			VARCHAR(MAX),
	@Pastors		VARCHAR(MAX)
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
				UPDATE	[config].[utbAboutPage]
				SET		[ActiveFlag] = 0
						,[LastModifyDate] = GETDATE()
						,[LastModifyUser] = @InsertUser
				WHERE 	[ActiveFlag] = 1

				INSERT INTO [config].[utbAboutPage] ([History],[Mision],[Vision],[Pastors],[InsertUser],[LastModifyUser])
				VALUES (@History, @Mision, @Vision, @Pastors, @InsertUser, @InsertUser)
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
