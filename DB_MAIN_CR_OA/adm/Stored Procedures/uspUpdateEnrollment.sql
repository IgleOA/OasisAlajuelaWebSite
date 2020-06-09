-- ======================================================================
-- Name: [adm].[uspUpdateEnrollment]
-- Desc: Se utiliza para actualizar un proceso de matricula
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 03/28/2020
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [adm].[uspUpdateEnrollment]
	@EnrollmentID	INT,
	@InsertUser		VARCHAR(50),
	@StartDate		DATETIME = NULL,
	@EndDate		DATETIME = NULL,
	@ActiveFlag		BIT = NULL
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
				UPDATE	[config].[utbEnrollments] 
				SET		[OpenRegister]		= ISNULL(@StartDate,[OpenRegister])
						,[CloseRegister]	= ISNULL(@EndDate,[CloseRegister])
						,[ActiveFlag]		= ISNULL(@ActiveFlag,[ActiveFlag])
						,[LastModifyDate]	= GETDATE()
						,[LastModifyUser]	= @InsertUser
				WHERE	[EnrollmentID] = @EnrollmentID				
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