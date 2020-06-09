-- ======================================================================
-- Name: [config].[ReadEnrolledUsers]
-- Desc: Retorna los usuarios matriculados
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 3/13/2020
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [config].[ReadEnrolledUsers]
	@RegisterID		INT = NULL,
	@EnrollmentID	INT	= NULL
AS 
    BEGIN
        SET NOCOUNT ON
        BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT

            -- =======================================================
				IF(@RegisterID IS NOT NULL)
					BEGIN
						SELECT	[RegisterID]
								,[UserID]
								,[FullName]
								,[PhoneNumber]
								,[ApprovalFlag]
						FROM	[config].[utbRegistrations]
						WHERE	[RegisterID] = @RegisterID
					END
				ELSE
					BEGIN
						SELECT	[RegisterID]
								,[UserID]
								,[FullName]
								,[PhoneNumber]
								,[ApprovalFlag]
						FROM	[config].[utbRegistrations]
						WHERE	[EnrollmentID] = @EnrollmentID
								AND [ActiveFlag] = 1
					END
			-- =======================================================

        END TRY
        BEGIN CATCH

            SELECT  @lErrorMessage = ERROR_MESSAGE() ,
                    @lErrorSeverity = ERROR_SEVERITY() ,
                    @lErrorState = ERROR_STATE()       

            RAISERROR (@lErrorMessage, @lErrorSeverity, @lErrorState);
        END CATCH
    END
    SET NOCOUNT OFF