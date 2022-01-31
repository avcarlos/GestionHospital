CREATE TABLE [dbo].[ExamenesCita]
(
	[IdExamenesCita]		INT IDENTITY(1,1)	NOT NULL,
	[IdCita]				INT					NOT NULL,
	[IdExamen]				INT					NULL,
	[Indicaciones]			VARCHAR(150)		NULL,
	CONSTRAINT [PK_ExamenesCita] PRIMARY KEY (IdExamenesCita),
	CONSTRAINT [FK_Cita_ExamenesCita] FOREIGN KEY (IdCita) REFERENCES Cita(IdCita)
)
