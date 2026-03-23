[Code]

var
  ImportIniPath: String;

function TryImportConfig(Path: String): Boolean;
var
  MonitorConfigPath: String;
  ExpectedHash: String;
  ConfigHash: String;

begin
  Result := False;

  ImportIniPath := Path;
  MonitorConfigPath := ExpandConstant(MONITOR_CONFIG_PATH);

  if FileExists(MonitorConfigPath) then
  begin
    ExpectedHash := Uppercase(GetIniString('config:monitor.xml', 'SHA256', '?', ImportIniPath));
    ConfigHash := Uppercase(GetSHA256OfFile(MonitorConfigPath));

    Result := ExpectedHash = ConfigHash;
  end;

end;