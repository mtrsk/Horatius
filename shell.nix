{ pkgs ? import ./pinned-nixpkgs.nix {}
, lib ? pkgs.stdenv.lib
}:

pkgs.stdenv.mkDerivation rec {
  name = "dev-shell";

  buildInputs = with pkgs; [
    binutils
    fontconfig
    libgdiplus
    mono6
    xorg.libX11
    xorg.libX11.dev
    zlib
  ];

  LD_LIBRARY_PATH = "${lib.makeLibraryPath buildInputs}";
  DISPLAY=":0";
}
