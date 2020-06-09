-- ======================================================================
-- Name: [adm].[uspAddRegistration]
-- Desc: Se utiliza para matricular un usuario
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 03/28/2020
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [adm].[uspAddRegistration]
	@InsertUser		VARCHAR(50),
	@UserID			INT,
	@EnrollmentID	INT,
	@FullName		VARCHAR(100),
	@PhoneNumber	VARCHAR(100)
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
				DECLARE @RegisterID INT
				
				SELECT	@RegisterID = [RegisterID]
				FROM	[config].[utbRegistrations]
				WHERE	[UserID] = @UserID
						AND [EnrollmentID] = @EnrollmentID

				IF(@RegisterID > 0)
					BEGIN
						UPDATE	[config].[utbRegistrations]
						SET		[ActiveFlag] = 1
								,[RegisterBy] = @InsertUser
						WHERE	[RegisterID] = @RegisterID
					END
				ELSE
					BEGIN
						INSERT INTO [config].[utbRegistrations] ([UserID],[FullName],[PhoneNumber],[EnrollmentID],[RegisterBy])
						VALUES (@UserID,@FullName,@PhoneNumber,@EnrollmentID,@InsertUser)
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