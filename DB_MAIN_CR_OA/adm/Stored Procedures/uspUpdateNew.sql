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
	@Title			VARCHAR(50) = NULL,
	@Description	VARCHAR(MAX) = NULL,
	@BannerData		VARBINARY(MAX) = NULL,
	@BannerExt		VARCHAR(10) = NULL	
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
						UPDATE	[config].[utbNews] 
						SET		 [Title]		= @Title
								,[Description]	= @Description	
								,[BannerData]	= @BannerData
								,[BannerExt]	= REPLACE(@BannerExt,'.','')
								,[ActiveFlag]		= 1
								,[LastModifyDate]	= @InsertDate
								,[LastModifyUser]	= @User
						WHERE	[NewID] = @NewID
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
