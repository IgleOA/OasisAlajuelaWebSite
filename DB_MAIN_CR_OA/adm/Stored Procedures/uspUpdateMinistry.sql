-- ======================================================================
-- Name: [adm].[uspUpdateMinistry]
-- Desc: Se utiliza para actualizar la información de un ministerio
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 03/28/2020
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [adm].[uspUpdateMinistry]
	@InsertUser		VARCHAR(50),
	@MinistryID		INT,
	@Name			VARCHAR(50) = NULL,
	@Description	VARCHAR(MAX) = NULL,
	@Image			VARBINARY(MAX) = NULL,
	@ImageExt		VARCHAR(10) = NULL,
	@ActionLink		VARCHAR(50) = NULL,
	@UpdateType		VARCHAR(10) = NULL
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
				IF(@UpdateType = 'DISABLE')
					BEGIN
						UPDATE	[config].[utbMinistries]
						SET		[ActiveFlag] =	0
								,[LastModifyDate] = GETDATE()
								,[LastModifyUser] = @InsertUser
						WHERE	[MinistryID] = @MinistryID
					END
				ELSE
					BEGIN
						UPDATE	[config].[utbMinistries]
						SET		[Name] =	@Name
								,[Description] = @Description
								,[Image] = ISNULL(@Image,[Image])
								,[ImageExt] = ISNULL(@ImageExt,[ImageExt])
								,[ActionLink] = @ActionLink
								,[LastModifyDate] = GETDATE()
								,[LastModifyUser] = @InsertUser
						WHERE	[MinistryID] = @MinistryID
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
