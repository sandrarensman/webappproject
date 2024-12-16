$(document).ready(function() {
    $('.available-student-dropdown').on('change', function() {
        let selectedStudentId = $(this).val();

        // Ensure selectedStudentId is treated as a string
        selectedStudentId = String(selectedStudentId);

        // Check if the student is already in the added list
        let isStudentAlreadyAdded = $('#addedStudentsList tr').toArray().some(function(row) {
            // Ensure data('student-id') is treated as a string as well
            return String($(row).data('student-id')) === selectedStudentId;
        });

        if (selectedStudentId && !isStudentAlreadyAdded) {
            let selectedStudentName = $(this).find('option:selected').text();
            let listItem = `<tr data-student-id="${selectedStudentId}">
                                        <td>
                                            <input type="checkbox" name="SelectedStudentIds" value="${selectedStudentId}" checked />
                                            ${selectedStudentName}
                                        </td>
                                    </tr>`;
            $('#addedStudentsList').append(listItem);

            // Update hidden input with the selected student IDs
            let selectedIds = [];
            $('#CurrentStudents input[type="checkbox"]:checked').each(function() {
                selectedIds.push($(this).val());
            });
            $('#HiddenSelectedStudentIds').val(selectedIds.join(','));

            // Clear the dropdown after selection
            $(this).val('').trigger('change');
        } else if (isStudentAlreadyAdded) {
            alert('This student has already been added to the group.');
        }
    });
});
