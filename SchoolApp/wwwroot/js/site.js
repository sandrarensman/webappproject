$(document).ready(function() {
    let addedStudentsHtml = '<h5>Students to Add</h5>';

    // Update the header dynamically
    function updateAddedStudentsHeader() {
        if ($('#addedStudentsList tr').length > 0) {
            $('#addedStudentsHeader').html(addedStudentsHtml);
        } else {
            $('#addedStudentsHeader').html('');
        }
    }

    // Initialize header on page load
    updateAddedStudentsHeader();

    $('.available-student-dropdown').on('change', function () {
        let selectedStudentId = $(this).val();
        selectedStudentId = String(selectedStudentId);

        let addedStudentsList = $('#addedStudentsList tr');

        let isStudentAlreadyAdded = addedStudentsList.toArray().some(function (row) {
            return String($(row).data('student-id')) === selectedStudentId;
        });

        if (selectedStudentId && !isStudentAlreadyAdded) {
            let selectedStudentName = $(this).find('option:selected').text();
            let listItem = `<tr data-student-id="${selectedStudentId}">
                                <td>
                                    <input type="checkbox" name="AddedStudentIds" value="${selectedStudentId}" checked />
                                    ${selectedStudentName}
                                </td>
                            </tr>`;
            $('#addedStudentsList').append(listItem);

            // Update hidden input for AddedStudentIds
            let selectedAddedStudentIds = [];
            $('#addedStudentsList input[type="checkbox"][name="AddedStudentIds"]:checked').each(function () {
                selectedAddedStudentIds.push($(this).val());
            });
            $('#HiddenAddedStudentIds').val(selectedAddedStudentIds.join(','));

            updateAddedStudentsHeader();

            $(this).val('').trigger('change');
        } else if (isStudentAlreadyAdded) {
            alert('This student has already been added to the group.');
        }

        // Ensure that the hidden input for CurrentStudentIds is updated as well
        let selectedCurrentStudentIds = [];
        $('#CurrentStudents input[type="checkbox"][name="CurrentStudentIds"]:checked').each(function () {
            selectedCurrentStudentIds.push($(this).val());
        });
        $('#HiddenCurrentStudentIds').val(selectedCurrentStudentIds.join(','));
    });
});

document.addEventListener("DOMContentLoaded", function () {
    const tableBody = document.querySelector("table tbody");

    tableBody.addEventListener("click", function (e) {
        const row = e.target.closest('.clickable-row');
        if (!row) return;

        // Group rows
        if (row.classList.contains('group-row')) {
            const groupId = row.getAttribute("data-id");
            if (groupId) {
                e.preventDefault();
                window.scrollTo(0, 0);

                // Get group members
                fetch(`/Groups?handler=GroupStudents&id=${groupId}`)
                    .then(response => response.text())
                    .then(content => {
                        document.getElementById("students-container").innerHTML = content;
                    })
                    .catch(error => {
                        console.error("Error fetching group students:", error); // Debug
                        document.getElementById("students-container").innerHTML = "<p>Failed to load students.</p>";
                    });
            }
        }

        // Student en enrollment rows
        if (row.classList.contains('student-row') || row.classList.contains('enrollment-row')) {
            const href = row.getAttribute('data-href');
            if (href) {
                window.location.href = href;
            }
        }
    });

    // Delete icon on student and enrollment page
    const deleteLinks = document.querySelectorAll('.delete-link');
    deleteLinks.forEach(link => {
        link.addEventListener('click', function () {
            window.location.href = link.href;
        });
    });

    // Details and edit icons on group page
    const iconLinks = document.querySelectorAll('.details-link, .edit-link');
    iconLinks.forEach(link => {
        link.addEventListener('click', function (e) {
            e.stopPropagation();
        });
    });
});

