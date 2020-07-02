-- ======================================================================
-- Name: [adm].[uspAddServices]
-- Desc: Se utiliza para agregar la informacion de los servicios
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 03/27/2020
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [adm].[uspAddServices]
	@InsertUser		VARCHAR(50),
	@SVCIcon		VARCHAR(50),
	@SVCName		VARCHAR(50),
	@SVCDescription	VARCHAR(MAX),
	@SVCOrder		INT,
	@ControllerLink	VARCHAR(50) = NULL,
	@ActionLink		VARCHAR(50) = NULL,
    @Parameter		VARCHAR(50) = NULL
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
				INSERT INTO [config].[utbServices] ([ServiceName],[ServiceDescription],[ServiceIcon],[ActiveFlag],[Order],[ControllerLink],[ActionLink],[Parameter],[InsertUser],[LastModifyUser])
				VALUES (@SVCName, @SVCDescription, @SVCIcon, 1, @SVCOrder, @ControllerLink, @ActionLink,@Parameter, @InsertUser, @InsertUser)
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