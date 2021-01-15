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

				DECLARE @Table TABLE ([NewID] INT, [Slide] INT)
				DECLARE @Index INT = 1

				DECLARE CURSOR_ITEM CURSOR
				FOR	SELECT	[NewID]
					FROM	[config].[utbNews]
					WHERE	[ActiveFlag] = ISNULL(@ActiveFlag,[ActiveFlag])
							AND [NewID] = ISNULL(@NewID,[NewID])
					ORDER BY [LastModifyUser] DESC, [InsertDate] DESC
				OPEN CURSOR_ITEM;

				FETCH NEXT FROM CURSOR_ITEM INTO
					@NewID;

				WHILE @@FETCH_STATUS = 0
					BEGIN
						IF(@Index = 5)
							BEGIN
								INSERT INTO @Table ([NewID],[Slide])
								VALUES (@NewID,1)
								SET @Index = 2
							END
						ELSE
							BEGIN
								INSERT INTO @Table ([NewID],[Slide])
								VALUES (@NewID,@Index)
								SET @Index = @Index + 1
							END
						FETCH NEXT FROM CURSOR_ITEM INTO
							@NewID;
					END;
				CLOSE CURSOR_ITEM;

				DEALLOCATE CURSOR_ITEM;


				SELECT	MD.[NewID] 
						,N.[Title]
						,N.[Description]
						,N.[BannerPath]
						,N.[ShowFlag]
						,N.[ActiveFlag]
						,[Date]			= CONVERT(DATE,ISNULL(N.[LastModifyDate],N.[InsertDate]))
						,[Year]			= CONVERT(VARCHAR(4),YEAR(ISNULL(N.[LastModifyDate],N.[InsertDate])))
						,[Month]		= DATENAME(MONTH,ISNULL(N.[LastModifyDate],N.[InsertDate]))
						,[Day]			= CASE WHEN DATEPART(DAY,ISNULL(N.[LastModifyDate],N.[InsertDate])) <10 THEN '0' + CONVERT(VARCHAR(1),DATEPART(DAY,ISNULL(N.[LastModifyDate],N.[InsertDate])))
											   ELSE CONVERT(VARCHAR(2),DATEPART(DAY,ISNULL(N.[LastModifyDate],N.[InsertDate]))) END
						,MD.[Slide]
				FROM	@Table MD
						LEFT JOIN [config].[utbNews] N ON N.[NewID] = MD.[NewID]
				ORDER BY N.[LastModifyUser] DESC, N.[InsertDate] DESC
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