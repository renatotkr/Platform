namespace Carbon.Platform
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	[Table("TestRuns")]
	public class TestFixtureRun
	{
		[Column("testName"), Key]
		public string TestName { get; set; }
	
		[Column("id"), Key]
		public long Id { get; set; }

		[Column("browser")]
		public string Browser { get; set; }

		[Column("status")]
		public TestResult Status { get; set; }

		[Column("date")]
		public DateTime Date { get; set; }

		[Column("duration")]
		public int Duration { get; set; } // In ms

		[Column("passed")]
		public int Passed { get; set; }

		[Column("total")]
		public int Total { get; set; }

		[Column("module")]
		public string Module { get; set; } // To group smaller tests
	}
}

/*
Object 
duration: 2
failed: 0
module: undefined
name: "Triggers"
passed: 1
total: 1
__proto__: Object
*/