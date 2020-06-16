-- ======================================================================
-- Name: [adm].[uspUpdatePodcast]
-- Desc: Se utiliza para actualizar la informacion de un Podcast
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 03/27/2020
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [adm].[uspUpdatePodcast]
	@InsertUser		VARCHAR(50),
	@PodcastID		INT,
	@InsertDate		DATETIME,
	@ActionType		VARCHAR(10) = NULL,
	@Title			VARCHAR(50) = NULL,
	@Description	VARCHAR(MAX) = NULL,
	@BannerData		VARBINARY(MAX) = NULL,
	@BannerExt		VARCHAR(10) = NULL,
	@MinisterID		INT = NULL
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
						FROM	[config].[utbPodcasts]
						WHERE	[PodcastID] = @PodcastID

						IF(@Status = 1)
							BEGIN
								UPDATE	[config].[utbPodcasts] 
								SET		[ActiveFlag] = 0
										,[LastModifyDate] = @InsertDate
										,[LastModifyUser] = @InsertUser
								WHERE	[PodcastID] = @PodcastID
							END
						ELSE
							BEGIN
								UPDATE	[config].[utbPodcasts] 
								SET		[ActiveFlag] = 1
										,[LastModifyDate] = @InsertDate
										,[LastModifyUser] = @InsertUser
								WHERE	[PodcastID] = @PodcastID
							END
					END
				ELSE					
					BEGIN
						UPDATE	[config].[utbPodcasts] 
						SET		 [Title]		= ISNULL(@Title,[Title])
								,[Description]	= ISNULL(@Description,[Description])
								,[BannerData]	= @BannerData
								,[BannerExt]	= REPLACE(@BannerExt,'.','')
								,[MinisterID]	= ISNULL(@MinisterID,[MinisterID])
								,[ActiveFlag]	= 1
								,[LastModifyDate]	= @InsertDate
								,[LastModifyUser]	= @InsertUser
						WHERE	[PodcastID] = @PodcastID
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