-- ======================================================================
-- Name: [config].[uspReadBlogs]
-- Desc: Retorna los detalles de los Blogs
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 3/13/2020
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [config].[uspReadBlogs]
	@BlogID			INT = NULL,
	@HistoryFlag	BIT = NULL
AS 
    BEGIN
        SET NOCOUNT ON
        BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT

            -- =======================================================
				SET LANGUAGE Spanish
				IF(@BlogID IS NOT NULL)
					BEGIN
						SELECT	P.[BlogID]
								,P.[Title]
								,P.[KeyWord]
								,P.[Description]
								,P.[BannerPath]
								,P.[MinisterID]
								,[MinisterName]		= M.[Title] + ' ' + M.[FullName]
								,[MinisterPhoto]	= M.[Photo]
								,P.[ActiveFlag]
								,[Date]			= CONVERT(DATE,ISNULL(P.[LastModifyDate],P.[InsertDate]))
								,[Year]			= CONVERT(VARCHAR(4),YEAR(ISNULL(P.[LastModifyDate],P.[InsertDate])))
								,[Month]		= DATENAME(MONTH,ISNULL(P.[LastModifyDate],P.[InsertDate]))
								,[Day]			= CASE WHEN DATEPART(DAY,ISNULL(P.[LastModifyDate],P.[InsertDate])) <10 THEN '0' + CONVERT(VARCHAR(1),DATEPART(DAY,ISNULL(P.[LastModifyDate],P.[InsertDate])))
														ELSE CONVERT(VARCHAR(2),DATEPART(DAY,ISNULL(P.[LastModifyDate],P.[InsertDate]))) END
								,[Slide]		= ROW_NUMBER() OVER(ORDER BY P.[LastModifyUser] DESC, P.[InsertDate] DESC) - 1
						FROM	[config].[utbBlogs] P
								LEFT JOIN [config].[utbMinisters] M ON M.[MinisterID] = P.[MinisterID]
						WHERE	P.[BlogID] = @BlogID
					END
				ELSE 
					BEGIN
						IF(@HistoryFlag = 1)
							BEGIN
								SELECT	P.[BlogID]
										,P.[Title]
										,P.[KeyWord]
										,P.[Description]
										,P.[BannerPath]
										,P.[MinisterID]
										,[MinisterName]		= M.[Title] + ' ' + M.[FullName]
										,[MinisterPhoto]	= M.[Photo]
										,P.[ActiveFlag]
										,[Date]			= CONVERT(DATE,ISNULL(P.[LastModifyDate],P.[InsertDate]))
										,[Year]			= CONVERT(VARCHAR(4),YEAR(ISNULL(P.[LastModifyDate],P.[InsertDate])))
										,[Month]		= DATENAME(MONTH,ISNULL(P.[LastModifyDate],P.[InsertDate]))
										,[Day]			= CASE WHEN DATEPART(DAY,ISNULL(P.[LastModifyDate],P.[InsertDate])) <10 THEN '0' + CONVERT(VARCHAR(1),DATEPART(DAY,ISNULL(P.[LastModifyDate],P.[InsertDate])))
															   ELSE CONVERT(VARCHAR(2),DATEPART(DAY,ISNULL(P.[LastModifyDate],P.[InsertDate]))) END
										,[Slide]		= 1
								FROM	[config].[utbBlogs] P
										LEFT JOIN [config].[utbMinisters] M ON M.[MinisterID] = P.[MinisterID]
								ORDER BY P.[LastModifyUser] DESC, P.[InsertDate] DESC
							END
						ELSE
							BEGIN
								DECLARE @Table TABLE ([BlogID] INT, [Slide] INT)
								DECLARE @Index INT = 1

								DECLARE CURSOR_ITEM CURSOR
								FOR	SELECT	[BlogID]
									FROM	[config].[utbBlogs]
									WHERE	[ActiveFlag] = 1		
									ORDER BY [InsertDate] DESC
								OPEN CURSOR_ITEM;

								FETCH NEXT FROM CURSOR_ITEM INTO
									@BlogID;

								WHILE @@FETCH_STATUS = 0
									BEGIN
										IF(@Index = 5)
											BEGIN
												INSERT INTO @Table ([BlogID],[Slide])
												VALUES (@BlogID,1)
												SET @Index = 2
											END
										ELSE
											BEGIN
												INSERT INTO @Table ([BlogID],[Slide])
												VALUES (@BlogID,@Index)
												SET @Index = @Index + 1
											END
										FETCH NEXT FROM CURSOR_ITEM INTO
											@BlogID;
									END;
								CLOSE CURSOR_ITEM;

								DEALLOCATE CURSOR_ITEM;

								SELECT	MD.[BlogID]
										,P.[Title]
										,P.[KeyWord]
										,P.[Description]
										,P.[BannerPath]
										,P.[MinisterID]
										,[MinisterName]		= M.[Title] + ' ' + M.[FullName]
										,[MinisterPhoto]	= M.[Photo]
										,P.[ActiveFlag]
										,[Date]			= CONVERT(DATE,ISNULL(P.[LastModifyDate],P.[InsertDate]))
										,[Year]			= CONVERT(VARCHAR(4),YEAR(ISNULL(P.[LastModifyDate],P.[InsertDate])))
										,[Month]		= DATENAME(MONTH,ISNULL(P.[LastModifyDate],P.[InsertDate]))
										,[Day]			= CASE WHEN DATEPART(DAY,ISNULL(P.[LastModifyDate],P.[InsertDate])) <10 THEN '0' + CONVERT(VARCHAR(1),DATEPART(DAY,ISNULL(P.[LastModifyDate],P.[InsertDate])))
																ELSE CONVERT(VARCHAR(2),DATEPART(DAY,ISNULL(P.[LastModifyDate],P.[InsertDate]))) END
										,[Slide]		= CASE WHEN MD.[Slide] = 2 THEN 400
															   WHEN MD.[Slide] = 3 THEN 400
															   ELSE 800 END
								FROM	@Table MD
										LEFT JOIN [config].[utbBlogs] P ON MD.[BlogID] = P.[BlogID]
										LEFT JOIN [config].[utbMinisters] M ON M.[MinisterID] = P.[MinisterID]
								ORDER BY P.[InsertDate] DESC
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