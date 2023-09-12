#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

namespace ModularTanksPack
{
    public class TanksAssembler : MonoBehaviour
    {
        private const int TanksNumber = 5;
        private const int TankColorsNumber = 4;
        private const int TracksColorsNumber = 3;

        private const float TrackUVAnimationSpeed = 0.5f;
        private const float BigTankTowerScale = 1.25f;
        private const float BigTankTowerScaleInverse = 0.75f;
        private const int BigTankNumber = 5;

        private const string TowerPrefabsNameFormat ="Tower{0}_Color{1}.prefab";
        private const string BasePrefabsNameFormat ="Base{0}_Color{1}.prefab";

        private const string BasePrefabsFolderPathFormat = "Assets/Stylized Tanks Pack/Prefabs/Tank_v{0}/Bases/";
        private const string TowerPrefabsFolderPathFormat = "Assets/Stylized Tanks Pack/Prefabs/Tank_v{0}/Towers/";
        private const string TrackMaterialsPathFormat = "Assets/Stylized Tanks Pack/Materials/Tracks/TrackMaterial_v{0}.mat";


        [SerializeField]private UIIntSelector tankHullSelector;
        [SerializeField]private UIIntSelector tankTowerSelector;
        [SerializeField]private UIIntSelector tankColorSelector;
        [SerializeField]private UIIntSelector tracksColorSelector;

        [SerializeField]private Button selectRandomTankButton;

        [Space(10f)]
        [SerializeField]
        private Transform tankSpawnPivot;

        private GameObject loadedTank;

        private int selectedTankHullNumber;
        private int selectedTankTowerNumber;
        private int selectedTankColorNumber;
        private int selectedTrackColorNumber;

        private void Awake()
        {
            tankHullSelector.Init(1, TanksNumber);
            tankHullSelector.SeletedOptionChanged += SelectedHullChanged;

            tankTowerSelector.Init(1, TanksNumber);
            tankTowerSelector.SeletedOptionChanged += SelectedTowerChanged;

            tankColorSelector.Init(1, TankColorsNumber);
            tankColorSelector.SeletedOptionChanged += SelectedMainColorChanged;

            tracksColorSelector.Init(1, TracksColorsNumber);
            tracksColorSelector.SeletedOptionChanged += SelectedTracksColorChanged;

            selectRandomTankButton.onClick.AddListener(SpawnRandomTank);
        }

        private void Start()
        {
            SpawnRandomTank();
        }

        private void SpawnRandomTank()
        {
            selectedTankHullNumber = Random.Range(1, TanksNumber + 1);
            selectedTankTowerNumber = Random.Range(1, TanksNumber + 1);
            selectedTankColorNumber = Random.Range(1, TankColorsNumber + 1);
            selectedTrackColorNumber = Random.Range(1, TracksColorsNumber + 1);

            tankHullSelector.SetSelectedOption(selectedTankHullNumber);
            tankTowerSelector.SetSelectedOption(selectedTankTowerNumber);
            tankColorSelector.SetSelectedOption(selectedTankColorNumber);
            tracksColorSelector.SetSelectedOption(selectedTrackColorNumber);

            SpawnTank(selectedTankHullNumber, selectedTankTowerNumber, selectedTankColorNumber, selectedTrackColorNumber);
        }

        private void SelectedHullChanged(int selectedNumber)
        {
            this.selectedTankHullNumber = selectedNumber;
            SpawnTank(selectedTankHullNumber, selectedTankTowerNumber, selectedTankColorNumber, selectedTrackColorNumber);
        }

        private void SelectedTowerChanged(int selectedNumber)
        {
            this.selectedTankTowerNumber = selectedNumber;
            SpawnTank(selectedTankHullNumber, selectedTankTowerNumber, selectedTankColorNumber, selectedTrackColorNumber);
        }

        private void SelectedMainColorChanged(int selectedNumber)
        {
            selectedTankColorNumber = selectedNumber;
            SpawnTank(selectedTankHullNumber, selectedTankTowerNumber, selectedTankColorNumber, selectedTrackColorNumber);
        }

        private void SelectedTracksColorChanged(int selectedNumber)
        {
            selectedTrackColorNumber = selectedNumber;
            SpawnTank(selectedTankHullNumber, selectedTankTowerNumber, selectedTankColorNumber, selectedTrackColorNumber);
        }

        private void SpawnTank(int tankHullNumber, int tankTowerNumber, int tankColorNumber, int tracksColorNumber)
        {
            Clear();
            selectedTankHullNumber = tankHullNumber;
            selectedTankTowerNumber = tankTowerNumber;
            selectedTankColorNumber = tankColorNumber;
            selectedTrackColorNumber = tracksColorNumber;
            var tankBasePrefab = AssetDatabase.LoadAssetAtPath<GameObject>(string.Format(BasePrefabsFolderPathFormat, tankHullNumber)+ string.Format(BasePrefabsNameFormat, tankHullNumber, tankColorNumber));
            loadedTank = GameObject.Instantiate(tankBasePrefab, tankSpawnPivot, false);
            var trackUVAnimation = loadedTank.transform.GetChild(0).gameObject.AddComponent<TrackUVAnimation>();
            trackUVAnimation.SetSpeed(TrackUVAnimationSpeed);

            var tracksRenderer = loadedTank.transform.GetChild(0).GetComponent<MeshRenderer>();
            tracksRenderer.sharedMaterial =  AssetDatabase.LoadAssetAtPath<Material>(string.Format(TrackMaterialsPathFormat, tracksColorNumber));

            var towerPivot = loadedTank.transform.GetChild(1);
            var tankTowerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(string.Format(TowerPrefabsFolderPathFormat, tankTowerNumber)+ string.Format(TowerPrefabsNameFormat, tankTowerNumber, tankColorNumber));
            GameObject.Instantiate(tankTowerPrefab, towerPivot, false);

            if (tankTowerNumber == BigTankNumber && tankHullNumber != BigTankNumber)
            {
                towerPivot.localScale = Vector3.one * BigTankTowerScaleInverse;
            }
            else if (tankHullNumber == BigTankNumber && tankTowerNumber != BigTankNumber)
            {
                towerPivot.localScale = Vector3.one * BigTankTowerScale;
            }
        }

        private void Clear()
        {
            if (loadedTank != null)
            {
                GameObject.Destroy(loadedTank);
            }
        }
    }
}
#endif
