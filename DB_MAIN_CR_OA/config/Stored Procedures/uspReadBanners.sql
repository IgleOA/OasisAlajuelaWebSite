-- ======================================================================
-- Name: [config].[uspReadBanners]
-- Desc: Retorna los banner segun la locacion
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 3/13/2020
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [config].[uspReadBanners]
	@pLocation	VARCHAR(100) = NULL,
	@pActiveFlag BIT = NULL
AS 
    BEGIN
        SET NOCOUNT ON
        BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT

            -- =======================================================
				DECLARE @lLocationID INT = (SELECT [LocationID]
										    FROM   [config].[utbBannersLocation]
										    WHERE  [LocationName] = @pLocation)

				SELECT	B.[BannerID]
						,B.[BannerPath]
						,B.[BannerName]
						,B.[LocationID]
						,[Location]		=	L.[LocationName]
						,B.[ActiveFlag]
						,[Slide]		= ROW_NUMBER() OVER(ORDER BY B.[BannerName]) - 1
				FROM	[config].[utbBanners] B
						LEFT JOIN [config].[utbBannersLocation] L ON L.[LocationID] = B.[LocationID]
				WHERE	B.[LocationID] = ISNULL(@lLocationID,B.[LocationID])
						AND B.[ActiveFlag]  = ISNULL(@pActiveFlag,B.[ActiveFlag])
				ORDER BY L.[LocationName],B.[ActiveFlag] DESC
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