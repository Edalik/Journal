namespace Journal.Models;

class ClownTask
{
    public int task_id { get; set; }
    public string name { get; set; }
    public string text { get; set; }
    public string status { get; set; }
    public int worker_id { get; set; }
    public int teacher_id { get; set; }
    public string worker { get; set; }
    public string teacher { get; set; }
    public string image { get; set; }
    public string first_date { get; set; }
    public string second_date { get; set; }
}