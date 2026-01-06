import unittest
from Lab_2 import merge_arrays, smallest_common, search_in_matrix, can_obtain_B_from_A  

class TestAlgorithms(unittest.TestCase):

    # Тести для Задачі 1
    def test_merge_arrays_basic(self):
        self.assertEqual(merge_arrays([1, 2, 3], [2, 3, 4]), [1, 2, 2, 3, 3, 4])

    def test_merge_arrays_empty(self):
        self.assertEqual(merge_arrays([], [1, 2, 3]), [1, 2, 3])
        self.assertEqual(merge_arrays([1, 2, 3], []), [1, 2, 3])

    def test_merge_arrays_duplicates(self):
        self.assertEqual(merge_arrays([1, 1, 2], [1, 2, 2]), [1, 1, 1, 2, 2, 2])

    # Тести для Задачі 2
    def test_smallest_common_basic(self):
        self.assertEqual(smallest_common([1, 2, 3], [2, 3, 4], [0, 3, 4]), 3)

    def test_smallest_common_first_common(self):
        self.assertEqual(smallest_common([1, 2, 3], [1, 2, 4], [1, 3, 5]), 1)

    def test_smallest_common_single_element(self):
        self.assertEqual(smallest_common([1], [1], [1]), 1)

    # Тести для Задачі 3
    def test_search_in_matrix_found(self):
        matrix = [
            [1, 3, 5],
            [2, 4, 6],
            [3, 5, 7]
        ]
        self.assertTrue(search_in_matrix(matrix, 4))
        self.assertTrue(search_in_matrix(matrix, 1))  

    def test_search_in_matrix_not_found(self):
        matrix = [
            [1, 3, 5],
            [2, 4, 6],
            [3, 5, 7]
        ]
        self.assertFalse(search_in_matrix(matrix, 8))  
        self.assertFalse(search_in_matrix(matrix, 0))  

    def test_search_in_matrix_single_element(self):
        matrix = [[1]]
        self.assertTrue(search_in_matrix(matrix, 1))
        self.assertFalse(search_in_matrix(matrix, 2))

    # Тести для Задачі 4
    def test_can_obtain_B_from_A_basic(self):
        self.assertTrue(can_obtain_B_from_A([1, 2, 3, 4], [2, 4]))

    def test_can_obtain_B_from_A_exact_match(self):
        self.assertTrue(can_obtain_B_from_A([1, 2, 3], [1, 2, 3]))

    def test_can_obtain_B_from_A_impossible(self):
        self.assertFalse(can_obtain_B_from_A([1, 2, 3], [2, 4]))  
        self.assertFalse(can_obtain_B_from_A([1, 3], [1, 2]))  

    def test_can_obtain_B_from_A_empty_B(self):
        self.assertTrue(can_obtain_B_from_A([1, 2, 3], []))  

if __name__ == '__main__':
    unittest.main()