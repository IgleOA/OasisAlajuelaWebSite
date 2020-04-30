-- ======================================================================
-- Name: [adm].[uspUpdateBanner]
-- Desc: Se utiliza para actualizar la informacion de una banner
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 03/27/2020
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [adm].[uspUpdateBanner]
	@User			VARCHAR(50),
	@BannerID		INT,
	@BannerName		VARCHAR(50) = NULL,
	@LocationID		INT = NULL,
	@ActiveFlag		BIT = NULL
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
				IF (@BannerName IS NULL)
					BEGIN
						DECLARE @Status BIT
						SELECT	@Status = [ActiveFlag]
						FROM	[config].[utbBanners]
						WHERE	[BannerID] = @BannerID

						IF(@Status = 1)
							BEGIN
								UPDATE	[config].[utbBanners] 
								SET		[ActiveFlag] = 0
										,[LastModifyDate] = GETDATE()
										,[LastModifyUser] = @User
								WHERE	[BannerID] = @BannerID
							END
						ELSE
							BEGIN
								UPDATE	[config].[utbBanners] 
								SET		[ActiveFlag] = 1
										,[LastModifyDate] = GETDATE()
										,[LastModifyUser] = @User
								WHERE	[BannerID] = @BannerID
							END
					END
				ELSE
					BEGIN
						UPDATE	[config].[utbBanners] 
						SET		 [BannerName]		= @BannerName
								,[LocationID]			= @LocationID	
								,[ActiveFlag]		= @ActiveFlag
								,[LastModifyDate]	= GETDATE()
								,[LastModifyUser]	= @User
						WHERE	[BannerID] = @BannerID
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