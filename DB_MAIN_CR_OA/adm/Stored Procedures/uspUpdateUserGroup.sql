-- ======================================================================
-- Name: [adm].[uspUpdateUserGroup]
-- Desc: Se utiliza para actualizar un UserGroup
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 03/27/2020
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [adm].[uspUpdateUserGroup]
	@InsertUser		VARCHAR(50),
	@UserGroupID	INT = NULL,
	@UserID			INT = NULL,
	@GroupID		INT = NULL
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
				IF (@UserGroupID IS NULL)
					BEGIN
						SELECT	@UserGroupID = UG.[UserGroupID]
						FROM	[config].[utbUsersGroups] UG
						WHERE	UG.[GroupID] = @GroupID
								AND UG.[UserID] = @UserID
					END

				UPDATE	[config].[utbUsersGroups]
				SET		[ActiveFlag] = 0
						,[LastModifyDate] = GETDATE()
						,[LastModifyUser] = @InsertUser
				WHERE	[UserGroupID] = @UserGroupID
				
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

