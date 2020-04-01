-- ======================================================================
-- Name: [config].[uspReadNews]
-- Desc: Retorna los detalles de los Noticias
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 3/13/2020
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [config].[uspReadNews]
AS 
    BEGIN
        SET NOCOUNT ON
        BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT

            -- =======================================================
				SET LANGUAGE Spanish
				SELECT	[NewID]
						,[Title]
						,[Description]
						,[BannerData]
						,[BannerExt]
						,[ActiveFlag]
						,[Year]			= CONVERT(VARCHAR(4),YEAR([InsertDate]))
						,[Month]		= DATENAME(MONTH,[InsertDate])
						,[Day]			= CASE WHEN DATEPART(DAY,[InsertDate]) <10 THEN '0' + CONVERT(VARCHAR(1),DATEPART(DAY,[InsertDate]))
											   ELSE CONVERT(VARCHAR(2),DATEPART(DAY,[InsertDate])) END
				FROM	[config].[utbNews]
				WHERE	[ActiveFlag] = 1
				ORDER BY [LastModifyUser] DESC, [InsertDate] DESC
			-- =======================================================

        END TRY
        BEGIN CATCH

            SELECT  @lErrorMessage = ERROR_MESSAGE() ,
                    @lErrorSeverity = ERROR_SEVERITY() ,
                    @lErrorState = ERROR_STATE()       

            RAISERROR (@lErrorMessage, @lErrorSeverity, @lErrorState);
        END CATCH
    END
    SET NOCOUNT OFF
