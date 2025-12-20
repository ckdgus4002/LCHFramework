using UnityEditor;
using UnityEngine;

public abstract class VectorBoolDrawer : PropertyDrawer
{
    protected abstract GUIContent[] Labels { get; }
    
    
    
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // 메인 라벨을 그리고 값 영역만 확보
        var valueRect = EditorGUI.PrefixLabel(position, label);

        // 인덴트/라벨폭 보존
        var prevIndent = EditorGUI.indentLevel;
        var prevMixed = EditorGUI.showMixedValue;
        EditorGUI.indentLevel = 0;

        // 토글/라벨 레이아웃 설정
        var lineH = EditorGUIUtility.singleLineHeight;
        var toggleSize = 16f;               // 체크박스 정사각형 크기
        var spaceToggleToLabel = 2f;        // 체크박스-라벨 간격
        var spaceBetweenItems = 6f;         // X, Y, Z 항목 간 간격

        // 좌측에서부터 순서대로 배치
        var x = valueRect.x;
        var y = valueRect.y;

        if (0 < Labels.Length) DrawItem(property.FindPropertyRelative("x"), Labels[0]);
        if (1 < Labels.Length) DrawItem(property.FindPropertyRelative("y"), Labels[1]);
        if (2 < Labels.Length) DrawItem(property.FindPropertyRelative("z"), Labels[2]);
        if (3 < Labels.Length) DrawItem(property.FindPropertyRelative("w"), Labels[3]);
        
        // 유틸 함수: 한 항목(X/Y/Z) 그리기
        void DrawItem(SerializedProperty prop, GUIContent gc)
        {
            // 라벨 폭을 실제 텍스트 기준으로 계산(미니 라벨)
            Vector2 labelSize = EditorStyles.miniLabel.CalcSize(gc);
            var totalW = toggleSize + spaceToggleToLabel + labelSize.x;

            // 토글 체크박스
            var toggleRect = new Rect(x, y, toggleSize, lineH);
            // 멀티 선택 혼합 값 표시(항목별)
            EditorGUI.showMixedValue = prop.hasMultipleDifferentValues;
            bool newVal = EditorGUI.Toggle(toggleRect, prop.boolValue);

            // 라벨(클릭 영역을 넓히고 싶다면 ToggleLeft로 통합해도 됩니다)
            var labelRect = new Rect(x + toggleSize + spaceToggleToLabel, y, labelSize.x, lineH);
            EditorGUI.LabelField(labelRect, gc, EditorStyles.miniLabel);

            if (newVal != prop.boolValue) prop.boolValue = newVal;

            // 다음 항목 x 위치로 이동
            x += totalW + spaceBetweenItems;
        }
        
        // 복원
        EditorGUI.showMixedValue = prevMixed;
        EditorGUI.indentLevel = prevIndent;

        EditorGUI.EndProperty();
    }
}