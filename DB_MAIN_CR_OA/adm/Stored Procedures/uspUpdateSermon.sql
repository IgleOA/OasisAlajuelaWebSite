-- ======================================================================
-- Name: [adm].[uspUpdateSermon]
-- Desc: Se utiliza para actualizar la informacion de un sermon
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 03/27/2020
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [adm].[uspUpdateSermon]
	@User			VARCHAR(50),	
	@SermonID		INT,
	@Title			VARCHAR(100) = NULL,
	@Description	VARCHAR(MAX) = NULL,
	@Tags			VARCHAR(MAX) = NULL,
	@MinisterID		INT = NULL,
	@BannerData		VARBINARY(MAX) = NULL,
	@BannerExt		VARCHAR(10) = NULL,
	@SermonDate		DATETIME = NULL,
	@SermonURL		VARCHAR(500) = NULL	
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
				IF (@Title IS NULL)
					BEGIN
						DECLARE @Status BIT
						SELECT	@Status = [ActiveFlag]
						FROM	[config].[utbSermons]
						WHERE	[SermonID] = @SermonID

						IF(@Status = 1)
							BEGIN
								UPDATE	[config].[utbSermons] 
								SET		[ActiveFlag] = 0
										,[LastModifyDate] = GETDATE()
										,[LastModifyUser] = @User
								WHERE	[SermonID] = @SermonID
							END
						ELSE
							BEGIN
								UPDATE	[config].[utbSermons] 
								SET		[ActiveFlag] = 1
										,[LastModifyDate] = GETDATE()
										,[LastModifyUser] = @User
								WHERE	[SermonID] = @SermonID
							END
					END
				ELSE
					BEGIN
						UPDATE	[config].[utbSermons] 
						SET		 [Title]		= @Title
								,[Description]	= @Description	
								,[Tags]			= @Tags
								,[MinisterID]	= @MinisterID
								,[SermonDate]	= @SermonDate
								,[BackgroundImage]	= @BannerData
								,[BackgroundExt]	= REPLACE(@BannerExt,'.','')
								,[ActiveFlag]		= 1
								,[LastModifyDate]	= GETDATE()
								,[LastModifyUser]	= @User
						WHERE	[SermonID] = @SermonID
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