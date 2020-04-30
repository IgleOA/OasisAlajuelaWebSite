-- ======================================================================
-- Name: [adm].[uspAddUserGroup]
-- Desc: Se utiliza para usuarios a grupos
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 03/27/2020
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [adm].[uspAddUserGroup]
	@InsertUser		VARCHAR(50),
	@UserID			INT,
	@GroupID		INT
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
				DECLARE @UserGroupID INT

				SELECT	@UserGroupID = G.[UserGroupID]
				FROM	[config].[utbUsersGroups] G
				WHERE	G.[UserID] = @UserID
						AND G.[GroupID] = @GroupID

				IF(@UserGroupID IS NOT NULL)
					BEGIN
						UPDATE	[config].[utbUsersGroups]
						SET		[ActiveFlag] = 1
								,[LastModifyDate] = GETDATE()
								,[LastModifyUser] = @InsertUser
						WHERE	[UserGroupID] = @UserGroupID
					END
				ELSE
					BEGIN
						INSERT INTO [config].[utbUsersGroups] ([UserID],[GroupID],[InsertUser],[LastModifyUser])
						VALUES (@UserID,@GroupID,@InsertUser,@InsertUser)
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