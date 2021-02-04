-- ======================================================================
-- Name: [config].[uspReadContactTypes]
-- Desc: Retorna las Tipos de Contacto
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 02/04/2021
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [config].[uspReadContactTypes]
    @ContactTypeID  INT = NULL,
    @ActiveFlag     BIT = NULL
AS 
    BEGIN
        SET NOCOUNT ON
        BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT

            -- =======================================================
				SELECT	[ContactTypeID]
                        ,[Type]
	                    ,[TypeName]
                        ,[TypeTitle]
	                    ,[TypeSubtitle]
	                    ,[Order]
                        ,[ActiveFlag]
                FROM    [config].[utbContactTypes]
                WHERE   [ActiveFlag] = ISNULL(@ActiveFlag,[ActiveFlag])
                        AND [ContactTypeID] = ISNULL(@ContactTypeID,[ContactTypeID])
                ORDER BY [Order]				
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


