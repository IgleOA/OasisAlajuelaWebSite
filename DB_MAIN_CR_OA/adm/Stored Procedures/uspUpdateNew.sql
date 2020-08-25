-- ======================================================================
-- Name: [adm].[uspUpdateNew]
-- Desc: Se utiliza para actualizar la informacion de una noticia
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 03/27/2020
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [adm].[uspUpdateNew]
	@User			VARCHAR(50),
	@NewID			INT,
	@InsertDate		DATETIME,
	@ActionType		VARCHAR(10) = NULL,
	@Title			VARCHAR(50) = NULL,
	@ShowFlag		BIT = NULL,
	@Description	VARCHAR(MAX) = NULL,
	@Banner			VARCHAR(500) = NULL
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
				IF (@ActionType = 'CHGST')
					BEGIN
						DECLARE @Status BIT
						SELECT	@Status = [ActiveFlag]
						FROM	[config].[utbNews]
						WHERE	[NewID] = @NewID

						IF(@Status = 1)
							BEGIN
								UPDATE	[config].[utbNews] 
								SET		[ActiveFlag] = 0
										,[LastModifyDate] = @InsertDate
										,[LastModifyUser] = @User
								WHERE	[NewID] = @NewID
							END
						ELSE
							BEGIN
								UPDATE	[config].[utbNews] 
								SET		[ActiveFlag] = 1
										,[LastModifyDate] = @InsertDate
										,[LastModifyUser] = @User
								WHERE	[NewID] = @NewID
							END
					END
				ELSE
					BEGIN
						IF (@ActionType = 'CHGVIS')
							BEGIN
								SELECT	@ShowFlag = [ShowFlag]
								FROM	[config].[utbNews]
								WHERE	[NewID] = @NewID

								IF(@ShowFlag = 1)
									BEGIN
										UPDATE	[config].[utbNews] 
										SET		[ShowFlag] = 0
												,[LastModifyDate] = @InsertDate
												,[LastModifyUser] = @User
										WHERE	[NewID] = @NewID
									END
								ELSE
									BEGIN
										UPDATE	[config].[utbNews] 
										SET		[ShowFlag] = 1
												,[LastModifyDate] = @InsertDate
												,[LastModifyUser] = @User
										WHERE	[NewID] = @NewID
									END
							END
						ELSE
							BEGIN
								UPDATE	[config].[utbNews] 
								SET		 [Title]		= @Title
										,[Description]	= @Description	
										,[BannerPath]	= @Banner
										,[ShowFlag]		= @ShowFlag
										,[InsertDate]	= @InsertDate
										,[ActiveFlag]	= 1
										,[LastModifyDate]	= @InsertDate
										,[LastModifyUser]	= @User
								WHERE	[NewID] = @NewID
							END
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