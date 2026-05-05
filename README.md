# Project : RTS Framework Development

### [Navigation]

상세한 설계 의도와 트러블 슈팅 사례는 **[노션 포트폴리오](https://www.notion.so/RTS-355fa4e779b7802bbd7ac542c99571a4?source=copy_link)**에서 확인하실 수 있습니다.  
이 README는 노션 문서의 기술적 구현체를 빠르게 확인하기 위한 가이드라인입니다.

| 기능 구분 | 핵심 스크립트 / 폴더 위치 | 주요 구현 및 트러블 슈팅 포인트 |
| :--- | :--- | :--- |
| **중앙 관리 시스템** | [GameManager.cs](https://github.com/fasd0114/RTS/blob/main/RTS/Assets/Scripts/Managers/GameManager.cs) | 중앙 관리형 자원 및 건물 리스트 |
| **건물 유닛 생산 시스템** | [/Scripts/Buildings](https://github.com/fasd0114/RTS/tree/main/RTS/Assets/Scripts/Buildings) | 유닛 생산 예약 및 큐 시스템 |
| **건설 시스템** | [BuildManager.cs](https://github.com/fasd0114/RTS/blob/main/RTS/Assets/Scripts/Managers/BuildManager.cs) | 그리드 기반 건물 배치 및 중첩 방지 |
| **미니맵 시스템** | [Scripts/Minimap](https://github.com/fasd0114/RTS/tree/main/RTS/Assets/Scripts/Minimap) | 오브젝트 아이콘 표시 및 지형 가시화 |
| **UI 시스템** | [Scripts/UI](https://github.com/fasd0114/RTS/tree/main/RTS/Assets/Scripts/UI)| 이벤트 기반 반응형 UI 시스템 |
| **유닛 로직** | [/Scripts/Units](https://github.com/fasd0114/RTS/tree/main/RTS/Assets/Scripts/Units) | NavMesh 및 FSM 기반 유닛 로직 |
| **유닛 다중 선택 및 명령 하달 시스템** | [/UnitSelectionManager.cs](https://github.com/fasd0114/RTS/blob/main/RTS/Assets/Scripts/Managers/UnitSelectionManager.cs) | 선택 유닛 시각화 및 명령 분기 |

<br>

> 본 프로젝트는 **Unity 2022.3.61f1** 환경에서 **C#** 을 사용하여 개발되었습니다.
