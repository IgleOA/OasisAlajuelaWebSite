-- ======================================================================
-- Name: [config].[uspReadEnrollments]
-- Desc: Retorna los procesos de matricula
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 3/13/2020
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [config].[uspReadEnrollments]
	@HistoryFlag	BIT = NULL,
	@EnrollmentID	INT = NULL
AS 
    BEGIN
        SET NOCOUNT ON
        BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT

            -- =======================================================
				IF(@HistoryFlag = 1)
					BEGIN
						SELECT	E.[EnrollmentID]
								,E.[GroupID]
								,G.[GroupName]
								,E.[OpenRegister]
								,E.[CloseRegister]
								,E.[ApprovalFlag]
						FROM	[config].[utbEnrollments] E
								LEFT JOIN [config].[utbGroups] G ON G.[GroupID] = E.[GroupID]
						WHERE	E.[ActiveFlag] = 1 
					END
				ELSE
					BEGIN
						IF(@EnrollmentID IS NOT NULL)
							BEGIN
								SELECT	E.[EnrollmentID]
										,E.[GroupID]
										,G.[GroupName]
										,E.[OpenRegister]
										,E.[CloseRegister]
										,E.[ApprovalFlag]
								FROM	[config].[utbEnrollments] E
										LEFT JOIN [config].[utbGroups] G ON G.[GroupID] = E.[GroupID]
								WHERE	[EnrollmentID] = @EnrollmentID
							END
						ELSE
							BEGIN
								SELECT	E.[EnrollmentID]
										,E.[GroupID]
										,G.[GroupName]
										,E.[OpenRegister]
										,E.[CloseRegister]
										,E.[ApprovalFlag]
								FROM	[config].[utbEnrollments] E
										LEFT JOIN [config].[utbGroups] G ON G.[GroupID] = E.[GroupID]
								WHERE	E.[ActiveFlag] = 1
										AND E.[ApprovalFlag] = 0
										AND E.[OpenRegister] <= DATEADD(HOUR,-6,GETDATE())
										AND E.[CloseRegister] > DATEADD(HOUR,-6,GETDATE())
							END
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