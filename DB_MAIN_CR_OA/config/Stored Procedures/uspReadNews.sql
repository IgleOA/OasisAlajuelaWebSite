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
	@ActiveFlag BIT = NULL,
	@NewID INT = NULL
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
						,[BannerPath]
						,[ShowFlag]
						,[ActiveFlag]
						,[Date]			= CONVERT(DATE,ISNULL([LastModifyDate],[InsertDate]))
						,[Year]			= CONVERT(VARCHAR(4),YEAR(ISNULL([LastModifyDate],[InsertDate])))
						,[Month]		= DATENAME(MONTH,ISNULL([LastModifyDate],[InsertDate]))
						,[Day]			= CASE WHEN DATEPART(DAY,ISNULL([LastModifyDate],[InsertDate])) <10 THEN '0' + CONVERT(VARCHAR(1),DATEPART(DAY,ISNULL([LastModifyDate],[InsertDate])))
											   ELSE CONVERT(VARCHAR(2),DATEPART(DAY,ISNULL([LastModifyDate],[InsertDate]))) END
				FROM	[config].[utbNews]
				WHERE	[ActiveFlag] = ISNULL(@ActiveFlag,[ActiveFlag])
						AND [NewID] = ISNULL(@NewID,[NewID])
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