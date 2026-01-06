from itertools import permutations

class TreeNode:
    def __init__(self, comparison=None, left=None, right=None, leaves=None):
        self.comparison = comparison
        self.left = left
        self.right = right
        self.leaves = leaves or []


def construct_decision_tree(elements):
    all_permutations = list(permutations(elements))
    size = len(elements)

    def build_tree(remaining_perms, used_comparisons=set()):
        if len(remaining_perms) == 1:
            return TreeNode(leaves=remaining_perms)

        for a in range(size):
            for b in range(a + 1, size):
                if (a, b) not in used_comparisons:
                    left_branch, right_branch = [], []
                    
                    for perm in remaining_perms:
                        (left_branch if perm[a] < perm[b] else right_branch).append(perm)
                    
                    updated_comparisons = used_comparisons | {(a, b)}
                    left_node = build_tree(left_branch, updated_comparisons) if left_branch else None
                    right_node = build_tree(right_branch, updated_comparisons) if right_branch else None
                    
                    return TreeNode(comparison=(a, b), left=left_node, right=right_node)
        
        return TreeNode(leaves=remaining_perms)

    return build_tree(all_permutations)


def extract_sorted_sequence(root):
    while root and not root.leaves:
        root = root.left  
    return root.leaves[0] if root else []


def visualize_tree(node, width=80, depth=0, position=0):
    if node is None:
        return []

    if node.leaves:
        return [(depth, position, str(node.leaves))]

    cmp_text = f"(arr[{node.comparison[0]}] ? arr[{node.comparison[1]}])"
    left_tree = visualize_tree(node.left, width, depth + 1, position - width // (2 ** (depth + 2)))
    right_tree = visualize_tree(node.right, width, depth + 1, position + width // (2 ** (depth + 2)))
    
    return [(depth, position, cmp_text)] + left_tree + right_tree


def print_tree_structure(root, width=80):
    tree_lines = visualize_tree(root, width)
    if not tree_lines:
        return
    
    max_depth = max(level for level, _, _ in tree_lines)
    structured_tree = {lvl: [] for lvl in range(max_depth + 1)}
    
    for lvl, pos, text in tree_lines:
        structured_tree[lvl].append((pos, text))
    
    for lvl in range(max_depth + 1):
        nodes = sorted(structured_tree[lvl], key=lambda x: x[0])
        line, last_pos = "", 0
        
        for pos, text in nodes:
            line += " " * (pos - last_pos) + text
            last_pos = pos + len(text)
        
        print(line.center(width))


input_array = [3, 1, 2]
decision_tree = construct_decision_tree(input_array)
print("\nДерево рішень для масиву", input_array)
print_tree_structure(decision_tree)
sorted_result = extract_sorted_sequence(decision_tree)
print("\nВідсортована послідовність:", sorted_result)
