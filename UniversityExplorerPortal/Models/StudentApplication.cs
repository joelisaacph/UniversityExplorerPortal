using System.ComponentModel.DataAnnotations;

namespace UniversityExplorerPortal.Models;

// Matches the JSON used by https://edutots.net/api/studentapplication
// The Data Annotations ([Required], [EmailAddress] ...) are used by the form for validation.
public class StudentApplication
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Student name is required.")]
    [StringLength(100, ErrorMessage = "Student name must be 100 characters or fewer.")]
    public string StudentName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email address is required.")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Country is required.")]
    public string Country { get; set; } = string.Empty;

    [Required(ErrorMessage = "University name is required.")]
    public string University { get; set; } = string.Empty;

    [Required(ErrorMessage = "Course name is required.")]
    public string Course { get; set; } = string.Empty;

    // The coursework asks for an application date. It is nullable so the date
    // picker can start empty, and so old records without a date still load.
    [Required(ErrorMessage = "Application date is required.")]
    public DateTime? ApplicationDate { get; set; }

    // The Edutots API stores a personal statement for every application.
    [Required(ErrorMessage = "Personal statement is required.")]
    [StringLength(500, ErrorMessage = "Personal statement must be 500 characters or fewer.")]
    public string PersonalStatement { get; set; } = string.Empty;
}
