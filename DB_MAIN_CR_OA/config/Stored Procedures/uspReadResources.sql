-- ======================================================================
-- Name: [config].[uspReadResources]
-- Desc: Retorna los detalles de los recursos
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 3/13/2020
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [config].[uspReadResources]
	@ResourceID		INT = NULL,	
	@HistoryFlag	BIT = NULL,	
	@ResourceTypeID INT = NULL,
	@Date			DATETIME = NULL
AS 
    BEGIN
        SET NOCOUNT ON
        BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT

            -- =======================================================
				IF(@ResourceID IS NOT NULL)
					BEGIN
						SELECT	S.[ResourceID]
								,S.[ResourceTypeID]
								,M.[TypeName] 		
								,S.[FileType]
								,S.[FileData]
								,S.[FileExt]
								,S.[FileName]
								,S.[FileURL]
								,S.[Description]
								,S.[EnableStart]
								,[ESTime]	= CONVERT(TIME,	S.[EnableStart])
								,S.[EnableEnd]
								,[EETime]	= CONVERT(TIME,	S.[EnableEnd])
								,S.[ActiveFlag]
						FROM	[config].[utbResources] S
								LEFT JOIN [config].[utbResourceTypes] M ON M.[ResourceTypeID] = S.[ResourceTypeID]
						WHERE	S.[ResourceID] = @ResourceID
					END
				ELSE
					BEGIN
						IF(@HistoryFlag = 1)
							BEGIN
								SELECT	S.[ResourceID]
										,S.[ResourceTypeID]
										,M.[TypeName] 		
										,S.[FileType]
										--,S.[FileData]
										,S.[FileExt]
										,S.[FileName]
										,[FileURL] = REPLACE(REPLACE(REPLACE(REPLACE(S.[FileURL],'https://youtu.be/','https://www.youtube.com/embed/'),'https://www.youtube.com/watch?v=','https://www.youtube.com/embed/'),'\',''),'view?usp=sharing','preview?rm=minimal')
										,S.[Description]
										,S.[EnableStart]
										,S.[EnableEnd]
										,S.[ActiveFlag]
								FROM	[config].[utbResources] S
										LEFT JOIN [config].[utbResourceTypes] M ON M.[ResourceTypeID] = S.[ResourceTypeID]
								WHERE	S.[ResourceTypeID] = @ResourceTypeID
							END
						ELSE
							BEGIN
								SELECT	S.[ResourceID]
										,S.[ResourceTypeID]
										,M.[TypeName] 		
										,S.[FileType]
										--,S.[FileData]
										,S.[FileExt]
										,S.[FileName]
										,[FileURL] = REPLACE(REPLACE(REPLACE(REPLACE(S.[FileURL],'https://youtu.be/','https://www.youtube.com/embed/'),'https://www.youtube.com/watch?v=','https://www.youtube.com/embed/'),'\',''),'view?usp=sharing','preview')
										,S.[Description]
										,S.[EnableStart]
										,S.[EnableEnd]
										,S.[ActiveFlag]
								FROM	[config].[utbResources] S
										LEFT JOIN [config].[utbResourceTypes] M ON M.[ResourceTypeID] = S.[ResourceTypeID]
								WHERE	S.[ActiveFlag] = 1
										AND S.ResourceTypeID = ISNULL(@ResourceTypeID,S.[ResourceTypeID])
										AND (@Date BETWEEN S.[EnableStart] AND S.[EnableEnd]
											 OR S.[EnableEnd] IS NULL)
								ORDER BY S.[InsertDate] DESC
							END
					END
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