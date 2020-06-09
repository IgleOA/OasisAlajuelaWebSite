-- ======================================================================
-- Name: [adm].[uspApproveEnrollment]
-- Desc: Se utiliza para aprobrar una matricula
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 03/28/2020
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [adm].[uspApproveEnrollment]
	@RegisterID		INT = NULL,
	@EnrollmentID	INT	= NULL
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
				IF(@RegisterID IS NOT NULL)
					BEGIN
						UPDATE	[config].[utbRegistrations] 
						SET		[ApprovalFlag] = 1
						WHERE	[RegisterID] = @RegisterID
						
						-- UPDATE FULL NAME
						UPDATE	U
						SET		U.[FullName] = R.[FullName]
						FROM	[config].[utbRegistrations] R
								INNER JOIN [adm].[utbUsers] U ON U.[UserID] = R.[UserID]
						WHERE	R.[RegisterID] = @RegisterID

						-- UPDATE MOBILE NUMBER
						UPDATE	U
						SET		U.[Mobile] = R.[PhoneNumber]
						FROM	[config].[utbRegistrations] R
								INNER JOIN [adm].[utbUsersProfile] U ON U.[UserID] = R.[UserID]
						WHERE	R.[RegisterID] = @RegisterID

						-- INSERT USER GROUP
						INSERT INTO [config].[utbUsersGroups] ([UserID],[GroupID])
						SELECT	R.[UserID]
								,E.[GroupID]
						FROM	[config].[utbRegistrations] R
								LEFT JOIN [config].[utbEnrollments] E ON E.[EnrollmentID] = R.[EnrollmentID]
						WHERE	R.[RegisterID] = @RegisterID

					END
				ELSE
					BEGIN
						UPDATE	[config].[utbRegistrations] 
						SET		[ApprovalFlag] = 1
						WHERE	[EnrollmentID] = @EnrollmentID	
								AND [ApprovalFlag]  = 0
								AND [ActiveFlag] = 1

						-- UPDATE FULL NAME
						UPDATE	U
						SET		U.[FullName] = R.[FullName]
						FROM	[config].[utbRegistrations] R
								INNER JOIN [adm].[utbUsers] U ON U.[UserID] = R.[UserID]
						WHERE	R.[EnrollmentID] = @EnrollmentID	
								AND R.[ApprovalFlag]  = 1
								AND R.[ActiveFlag] = 1

						-- UPDATE MOBILE NUMBER
						UPDATE	U
						SET		U.[Mobile] = R.[PhoneNumber]
						FROM	[config].[utbRegistrations] R
								INNER JOIN [adm].[utbUsersProfile] U ON U.[UserID] = R.[UserID]
						WHERE	R.[EnrollmentID] = @EnrollmentID	
								AND R.[ApprovalFlag]  = 1
								AND R.[ActiveFlag] = 1

						-- INSERT USER GROUP
						INSERT INTO [config].[utbUsersGroups] ([UserID],[GroupID])
						SELECT	R.[UserID]
								,E.[GroupID]
						FROM	[config].[utbRegistrations] R
								LEFT JOIN [config].[utbEnrollments] E ON E.[EnrollmentID] = R.[EnrollmentID]
						WHERE	R.[EnrollmentID] = @EnrollmentID	
								AND R.[ApprovalFlag]  = 1	
								AND R.[ActiveFlag] = 1

						-- UPDATE ENROLLMENTS TABLE
						UPDATE	[config].[utbEnrollments]
						SET		[ApprovalFlag] = 1
						WHERE	[EnrollmentID] = @EnrollmentID	
					END
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