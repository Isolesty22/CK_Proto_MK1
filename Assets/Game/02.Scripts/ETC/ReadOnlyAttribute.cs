using UnityEngine;

/// <summary>
/// 인스펙터에서 필드를 '읽기 전용'으로 표시하여 값을 수정할 수 없습니다. 
/// 이 속성을 사용하면 다른 CustomPropertyDrawers가 작동하지 않습니다.
/// </summary>
/// <seealso cref="BeginReadOnlyGroupAttribute"/>
/// <seealso cref="EndReadOnlyGroupAttribute"/>
public class ReadOnlyAttribute : PropertyAttribute { }

/// <summary>
/// 하나 이상의 필드를 인스펙터에서 '읽기 전용'으로 표시하여 값을 수정할 수 없습니다.
/// <see cref="EndReadOnlyGroupAttribute"/>와 함께 사용하여 그룹을 닫을 수 있습니다.
/// 이 속성을 사용하면 CustomPropertyDrawers가 정상적으로 작동합니다.
/// </summary>
/// <seealso cref="EndReadOnlyGroupAttribute"/>
/// <seealso cref="ReadOnlyAttribute"/>
public class BeginReadOnlyGroupAttribute : PropertyAttribute { }

/// <summary>
/// <see cref="BeginReadOnlyGroupAttribute"/>와 함께 사용합니다.
/// '읽기 전용' 그룹을 닫을 수 있습니다.
/// 또한, Atrribute 태그는 원래 다음 필드에 적용되는 것이기 때문에, 다음 필드가 없을 경우 생략할 수 있습니다.
/// </summary>
/// <seealso cref="BeginReadOnlyGroupAttribute"/>
/// <seealso cref="ReadOnlyAttribute"/>
public class EndReadOnlyGroupAttribute : PropertyAttribute { }