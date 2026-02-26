USE [Student_Management_System]
GO

/****** Object:  Table [dbo].[Course_Sem_Mapping]    Script Date: 25-02-2026 09:33:47 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Course_Sem_Mapping](
	[Course_Sem_Map_Id] [int] IDENTITY(1,1) NOT NULL,
	[Course_Id] [int] NOT NULL,
	[Sem_Id] [int] NOT NULL,
	[Created_By] [int] NOT NULL,
	[Created_Date] [datetime] NULL,
	[Modified_By] [int] NULL,
	[Modified_Date] [datetime] NULL,
	[Obsolete] [char](1) NULL,
 CONSTRAINT [PK__Course_S__9034B089B1008415] PRIMARY KEY CLUSTERED 
(
	[Course_Sem_Map_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_Course_Sem] UNIQUE NONCLUSTERED 
(
	[Course_Id] ASC,
	[Sem_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Course_Sem_Mapping] ADD  CONSTRAINT [DF__Course_Se__Creat__5070F446]  DEFAULT (getdate()) FOR [Created_Date]
GO

ALTER TABLE [dbo].[Course_Sem_Mapping] ADD  CONSTRAINT [DF__Course_Se__Obsol__5165187F]  DEFAULT ('N') FOR [Obsolete]
GO

