# JiaSyuanLibrary.NetFramework
TargetFrameworkVersion:4.8.1
---

## AutoMappingHelper

AutoMappingHelper 是一個以 AutoMapper 為核心的封裝工具，支援物件映射、集合轉換、Profile 註冊與快取等功能。簡化日常開發中的 DTO / ViewModel 轉換流程，並提升映射可讀性與擴充性。

---

### ✨ 特點特色

- ✅ 支援單一物件、集合、合併來源映射
- ✅ 結合 MappingProfileRegistry 進行 Profile 註冊管理
- ✅ 透過 MapperFactory 實現 IMapper 快取與延遲建立
- ✅ 相容 AutoMapper 最新版本（含 ILoggerFactory）

---

### 🔧 安裝套件

請確保已安裝 AutoMapper 相關套件：

```bash
dotnet add package AutoMapper
dotnet add package Microsoft.Extensions.Logging.Abstractions
