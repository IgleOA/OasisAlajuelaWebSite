-- ======================================================================
-- Name: [adm].[uspUpdateUserProfile]
-- Desc: Se utiliza para actualizar la información de un usuario
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 04/02/2020
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [adm].[uspUpdateUserProfile]
	@ActionType		VARCHAR(10)
	,@InsertUser	VARCHAR(100)
	,@UserID		INT
	,@Photo			VARCHAR(500)	= NULL
	,@Phone			VARCHAR(50)		= NULL
	,@Mobile		VARCHAR(50)		= NULL
	,@Country		VARCHAR(100)	= NULL
	,@State			VARCHAR(100)	= NULL
	,@City			VARCHAR(100)	= NULL
	,@Facebook		VARCHAR(100)	= NULL
	,@Twitter		VARCHAR(100)	= NULL
	,@Snapchat		VARCHAR(100)	= NULL
	,@Instragram	VARCHAR(100)	= NULL
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
				IF EXISTS (SELECT [UserID] FROM [adm].[utbUsersProfile] WHERE [UserID] = @UserID)
					BEGIN
						IF(@ActionType = 'PHOTO')
							BEGIN
								UPDATE	[adm].[utbUsersProfile]
								SET		[PhotoPath] = @Photo
										,[LastModifyDate] = GETDATE()
										,[LastModifyUser] = @InsertUser
								WHERE	[UserID] = @UserID
								SELECT [UserID] = @UserID
							END
						ElSE
							BEGIN
								IF(@ActionType = 'CONTACT')
									BEGIN
										UPDATE	[adm].[utbUsersProfile]
										SET		[Phone]		= @Phone
												,[Mobile]	= @Mobile
												,[Country]	= @Country
												,[State]	= @State
												,[City]		= @City
												,[LastModifyDate]	= GETDATE()
												,[LastModifyUser]	= @InsertUser
										WHERE	[UserID] = @UserID
										SELECT [UserID] = @UserID
									END
								ELSE
									BEGIN
										UPDATE	[adm].[utbUsersProfile]
										SET		[Facebook]		= @Facebook
												,[Twitter]		= @Twitter
												,[Snapchat]		= @Snapchat
												,[Instragram]	= @Instragram
												,[LastModifyDate]	= GETDATE()
												,[LastModifyUser]	= @InsertUser
										WHERE [UserID] = @UserID
										SELECT [UserID] = @UserID
									END
							END
					END
				ELSE
					BEGIN
						INSERT INTO [adm].[utbUsersProfile]	([UserID],[PhotoPath],[Phone],[Mobile],[Facebook],[Twitter],[Snapchat],[Instragram],[Country],[State],[City],[CreationUser],[LastModifyUser])
						VALUES (@UserID,@Photo,@Phone,@Mobile,@Facebook,@Twitter,@Snapchat,@Instragram,@Country,@State,@City,@InsertUser,@InsertUser)	
						SELECT [UserID] = @UserID
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