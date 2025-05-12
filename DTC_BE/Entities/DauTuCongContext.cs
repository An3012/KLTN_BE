using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DTC_BE.Entities;

public partial class DauTuCongContext : DbContext
{
    public DauTuCongContext()
    {
    }

    public DauTuCongContext(DbContextOptions<DauTuCongContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DmChuDauTu> DmChuDauTus { get; set; }

    public virtual DbSet<DmDuAn> DmDuAns { get; set; }

    public virtual DbSet<DmQuanHuyen> DmQuanHuyens { get; set; }

    public virtual DbSet<DmTinhThanh> DmTinhThanhs { get; set; }

    public virtual DbSet<DmXaPhuong> DmXaPhuongs { get; set; }

    public virtual DbSet<HtMenu> HtMenus { get; set; }

    public virtual DbSet<HtNguoiDung> HtNguoiDungs { get; set; }

    public virtual DbSet<HtNhatKyHeThong> HtNhatKyHeThongs { get; set; }

    public virtual DbSet<HtNhomQuyen> HtNhomQuyens { get; set; }

    public virtual DbSet<HtQuyen> HtQuyens { get; set; }

    public virtual DbSet<HtQuyenNhomQuyen> HtQuyenNhomQuyens { get; set; }

    public virtual DbSet<HtThamSoHeThong> HtThamSoHeThongs { get; set; }

    public virtual DbSet<QuanLyThuTucBaoCaoTongHopVeGoiThau> QuanLyThuTucBaoCaoTongHopVeGoiThaus { get; set; }

    public virtual DbSet<QuanLyThuTucCnkq> QuanLyThuTucCnkqs { get; set; }

    public virtual DbSet<QuanLyThuTucFile> QuanLyThuTucFiles { get; set; }

    public virtual DbSet<QuanLyThuTucNoiBoBaoCaoTongHop> QuanLyThuTucNoiBoBaoCaoTongHops { get; set; }

    public virtual DbSet<QuanLyThuTucNoiBoBaoCaoTongHopChiTiet> QuanLyThuTucNoiBoBaoCaoTongHopChiTiets { get; set; }

    public virtual DbSet<QuanLyThuTucNoiBoChuyenThuLy> QuanLyThuTucNoiBoChuyenThuLies { get; set; }

    public virtual DbSet<QuanLyThuTucNoiBoDuAnDtc> QuanLyThuTucNoiBoDuAnDtcs { get; set; }

    public virtual DbSet<QuanLyThuTucNoiBoDuAnDtcKqth> QuanLyThuTucNoiBoDuAnDtcKqths { get; set; }

    public virtual DbSet<QuanLyThuTucNoiBoDuAnDtcNguonVon> QuanLyThuTucNoiBoDuAnDtcNguonVons { get; set; }

    public virtual DbSet<QuanLyThuTucNoiBoDuAnDtcPhieuXuLy> QuanLyThuTucNoiBoDuAnDtcPhieuXuLies { get; set; }

    public virtual DbSet<QuanLyThuTucNoiBoDuAnDtcTienDoXuLy> QuanLyThuTucNoiBoDuAnDtcTienDoXuLies { get; set; }

    public virtual DbSet<ThuTucNbLuaChonNhaThauPhanChiaGoiThau> ThuTucNbLuaChonNhaThauPhanChiaGoiThaus { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=116.98.222.172;Database=DTC_DEV;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DmChuDauTu>(entity =>
        {
            entity.ToTable("DM_CHU_DAU_TU");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.DiaChi)
                .HasMaxLength(500)
                .HasColumnName("DIA_CHI");
            entity.Property(e => e.Email)
                .HasMaxLength(500)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Loai)
                .HasComment("Để phân biệt là ủy ban nhân dân huyện hoặc ko 0 là huyện 1; khác huyện")
                .HasColumnName("LOAI");
            entity.Property(e => e.MaSoThue)
                .HasMaxLength(50)
                .HasColumnName("MA_SO_THUE");
            entity.Property(e => e.NgayCapNhat)
                .HasColumnType("datetime")
                .HasColumnName("NGAY_CAP_NHAT");
            entity.Property(e => e.NgayHoatDong)
                .HasColumnType("datetime")
                .HasColumnName("NGAY_HOAT_DONG");
            entity.Property(e => e.NgayTao)
                .HasColumnType("datetime")
                .HasColumnName("NGAY_TAO");
            entity.Property(e => e.NguoiCapNhat)
                .HasMaxLength(50)
                .HasColumnName("NGUOI_CAP_NHAT");
            entity.Property(e => e.NguoiDaiDien)
                .HasMaxLength(200)
                .HasColumnName("NGUOI_DAI_DIEN");
            entity.Property(e => e.NguoiTao)
                .HasMaxLength(50)
                .HasColumnName("NGUOI_TAO");
            entity.Property(e => e.QuanHuyen)
                .HasMaxLength(50)
                .HasColumnName("QUAN_HUYEN");
            entity.Property(e => e.SoDienThoai)
                .HasMaxLength(50)
                .HasColumnName("SO_DIEN_THOAI");
            entity.Property(e => e.TenChuDauTu)
                .HasMaxLength(500)
                .HasColumnName("TEN_CHU_DAU_TU");
            entity.Property(e => e.TinhThanh)
                .HasMaxLength(50)
                .HasColumnName("TINH_THANH");
            entity.Property(e => e.XaPhuong)
                .HasMaxLength(50)
                .HasColumnName("XA_PHUONG");
        });

        modelBuilder.Entity<DmDuAn>(entity =>
        {
            entity.ToTable("DM_DU_AN");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.CoCauNguonVon)
                .HasMaxLength(50)
                .HasColumnName("CO_CAU_NGUON_VON");
            entity.Property(e => e.CreateAt)
                .HasColumnType("datetime")
                .HasColumnName("CREATE_AT");
            entity.Property(e => e.DiaDiemDauTu)
                .HasMaxLength(250)
                .HasColumnName("DIA_DIEM_DAU_TU");
            entity.Property(e => e.DmChuDauTuId)
                .HasMaxLength(50)
                .HasColumnName("DM_CHU_DAU_TU_ID");
            entity.Property(e => e.GhiChu)
                .HasMaxLength(250)
                .HasColumnName("GHI_CHU");
            entity.Property(e => e.HinhThucQuanLy)
                .HasMaxLength(50)
                .HasColumnName("HINH_THUC_QUAN_LY");
            entity.Property(e => e.IsXoa).HasColumnName("IS_XOA");
            entity.Property(e => e.MaDuAn)
                .HasMaxLength(150)
                .HasColumnName("MA_DU_AN");
            entity.Property(e => e.NangLucThietKe)
                .HasMaxLength(50)
                .HasColumnName("NANG_LUC_THIET_KE");
            entity.Property(e => e.NhomDuAn).HasColumnName("NHOM_DU_AN");
            entity.Property(e => e.SoBuocThietKe)
                .HasMaxLength(250)
                .HasColumnName("SO_BUOC_THIET_KE");
            entity.Property(e => e.TenDuAn).HasColumnName("TEN_DU_AN");
            entity.Property(e => e.ThoiGianThucHien)
                .HasMaxLength(50)
                .HasColumnName("THOI_GIAN_THUC_HIEN");
            entity.Property(e => e.TienDoThucHien)
                .HasMaxLength(50)
                .HasColumnName("TIEN_DO_THUC_HIEN");
            entity.Property(e => e.TongMucDauTu).HasColumnName("TONG_MUC_DAU_TU");

            entity.HasOne(d => d.DmChuDauTu).WithMany(p => p.DmDuAns)
                .HasForeignKey(d => d.DmChuDauTuId)
                .HasConstraintName("FK_DM_DU_AN_DM_CHU_DAU_TU");
        });

        modelBuilder.Entity<DmQuanHuyen>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("DM_QUAN_HUYEN");

            entity.Property(e => e.CreateAt)
                .HasColumnType("datetime")
                .HasColumnName("CREATE_AT");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(50)
                .HasColumnName("CREATE_BY");
            entity.Property(e => e.DmTinhThanhId)
                .HasMaxLength(50)
                .HasColumnName("DM_TINH_THANH_ID");
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.MaQuanHuyen)
                .HasMaxLength(100)
                .HasColumnName("MA_QUAN_HUYEN");
            entity.Property(e => e.TenQuanHuyen)
                .HasMaxLength(100)
                .HasColumnName("TEN_QUAN_HUYEN");
            entity.Property(e => e.UpdateAt)
                .HasColumnType("datetime")
                .HasColumnName("UPDATE_AT");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(50)
                .HasColumnName("UPDATE_BY");
        });

        modelBuilder.Entity<DmTinhThanh>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("DM_TINH_THANH");

            entity.Property(e => e.CreateAt)
                .HasColumnType("datetime")
                .HasColumnName("CREATE_AT");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(50)
                .HasColumnName("CREATE_BY");
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.MaTinhThanh)
                .HasMaxLength(100)
                .HasColumnName("MA_TINH_THANH");
            entity.Property(e => e.TenTinhThanh)
                .HasMaxLength(100)
                .HasColumnName("TEN_TINH_THANH");
            entity.Property(e => e.UpdateAt)
                .HasColumnType("datetime")
                .HasColumnName("UPDATE_AT");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(50)
                .HasColumnName("UPDATE_BY");
        });

        modelBuilder.Entity<DmXaPhuong>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("DM_XA_PHUONG");

            entity.Property(e => e.CreateAt)
                .HasColumnType("datetime")
                .HasColumnName("CREATE_AT");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(50)
                .HasColumnName("CREATE_BY");
            entity.Property(e => e.DmQuanHuyenId)
                .HasMaxLength(50)
                .HasColumnName("DM_QUAN_HUYEN_ID");
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.MaXaPhuong)
                .HasMaxLength(100)
                .HasColumnName("MA_XA_PHUONG");
            entity.Property(e => e.TenXaPhuong)
                .HasMaxLength(100)
                .HasColumnName("TEN_XA_PHUONG");
            entity.Property(e => e.UpdateAt)
                .HasColumnType("datetime")
                .HasColumnName("UPDATE_AT");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(50)
                .HasColumnName("UPDATE_BY");
        });

        modelBuilder.Entity<HtMenu>(entity =>
        {
            entity.ToTable("HT_MENU");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.Cap).HasColumnName("CAP");
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CODE");
            entity.Property(e => e.Icon)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ICON");
            entity.Property(e => e.IsActive).HasColumnName("IS_ACTIVE");
            entity.Property(e => e.Link)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("LINK");
            entity.Property(e => e.MoTa)
                .HasMaxLength(250)
                .HasColumnName("MO_TA");
            entity.Property(e => e.ParentCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PARENT_CODE");
            entity.Property(e => e.PhanHe).HasColumnName("PHAN_HE");
            entity.Property(e => e.RouterLink)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("ROUTER_LINK");
            entity.Property(e => e.TenMenu)
                .HasMaxLength(250)
                .HasColumnName("TEN_MENU");
            entity.Property(e => e.ThuTu).HasColumnName("THU_TU");
        });

        modelBuilder.Entity<HtNguoiDung>(entity =>
        {
            entity.ToTable("HT_NGUOI_DUNG");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.DiaChi)
                .HasMaxLength(500)
                .HasColumnName("DIA_CHI");
            entity.Property(e => e.DmChuDauTuId)
                .HasMaxLength(50)
                .HasColumnName("DM_CHU_DAU_TU_ID");
            entity.Property(e => e.DmDonViId)
                .HasMaxLength(50)
                .HasColumnName("DM_DON_VI_ID");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Expiresat)
                .HasColumnType("datetime")
                .HasColumnName("EXPIRESAT");
            entity.Property(e => e.GioiTinh)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasComment("0: nữ 1: nam")
                .HasColumnName("GIOI_TINH");
            entity.Property(e => e.HoTen)
                .HasMaxLength(150)
                .HasColumnName("HO_TEN");
            entity.Property(e => e.HtNhomQuyenId)
                .HasMaxLength(50)
                .HasColumnName("HT_NHOM_QUYEN_ID");
            entity.Property(e => e.Isrefreshtokenrevoked).HasColumnName("ISREFRESHTOKENREVOKED");
            entity.Property(e => e.LoaiTaiKhoan)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasComment("1: Chủ đầu tư 2: UBND huyện")
                .HasColumnName("LOAI_TAI_KHOAN");
            entity.Property(e => e.MatKhau)
                .HasMaxLength(50)
                .HasColumnName("MAT_KHAU");
            entity.Property(e => e.NgayCapNhat)
                .HasColumnType("datetime")
                .HasColumnName("NGAY_CAP_NHAT");
            entity.Property(e => e.NgayTao)
                .HasColumnType("datetime")
                .HasColumnName("NGAY_TAO");
            entity.Property(e => e.NguoiCapNhat)
                .HasMaxLength(50)
                .HasColumnName("NGUOI_CAP_NHAT");
            entity.Property(e => e.NguoiTao)
                .HasMaxLength(50)
                .HasColumnName("NGUOI_TAO");
            entity.Property(e => e.PhongBan).HasColumnName("PHONG_BAN");
            entity.Property(e => e.Refreshtoken)
                .HasMaxLength(250)
                .HasColumnName("REFRESHTOKEN");
            entity.Property(e => e.SoDienThoai)
                .HasMaxLength(50)
                .HasColumnName("SO_DIEN_THOAI");
            entity.Property(e => e.TenDangNhap)
                .HasMaxLength(50)
                .HasColumnName("TEN_DANG_NHAP");
            entity.Property(e => e.TrangThai).HasColumnName("TRANG_THAI");

            entity.HasOne(d => d.HtNhomQuyen).WithMany(p => p.HtNguoiDungs)
                .HasForeignKey(d => d.HtNhomQuyenId)
                .HasConstraintName("FK_HT_NGUOI_DUNG_HT_NHOM_QUYEN");
        });

        modelBuilder.Entity<HtNhatKyHeThong>(entity =>
        {
            entity.ToTable("HT_NHAT_KY_HE_THONG");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.IpNguoiDung)
                .HasMaxLength(50)
                .HasColumnName("IP_NGUOI_DUNG");
            entity.Property(e => e.LoaiChucNang)
                .HasMaxLength(250)
                .HasColumnName("LOAI_CHUC_NANG");
            entity.Property(e => e.MoTa)
                .HasMaxLength(500)
                .HasColumnName("MO_TA");
            entity.Property(e => e.NgayCapNhat)
                .HasColumnType("datetime")
                .HasColumnName("NGAY_CAP_NHAT");
            entity.Property(e => e.TenNguoiDung)
                .HasMaxLength(50)
                .HasColumnName("TEN_NGUOI_DUNG");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(250)
                .HasColumnName("TRANG_THAI");
        });

        modelBuilder.Entity<HtNhomQuyen>(entity =>
        {
            entity.ToTable("HT_NHOM_QUYEN");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.MoTa)
                .HasMaxLength(50)
                .HasColumnName("MO_TA");
            entity.Property(e => e.Ten)
                .HasMaxLength(50)
                .HasColumnName("TEN");
        });

        modelBuilder.Entity<HtQuyen>(entity =>
        {
            entity.ToTable("HT_QUYEN");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.Ma)
                .HasMaxLength(50)
                .HasColumnName("MA");
            entity.Property(e => e.MaCha)
                .HasMaxLength(50)
                .HasColumnName("MA_CHA");
            entity.Property(e => e.TenQuyen)
                .HasMaxLength(250)
                .HasColumnName("TEN_QUYEN");
        });

        modelBuilder.Entity<HtQuyenNhomQuyen>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_HT_QUYEN_NHOMQUYEN");

            entity.ToTable("HT_QUYEN_NHOM_QUYEN");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.HtNhomQuyenId)
                .HasMaxLength(50)
                .HasColumnName("HT_NHOM_QUYEN_ID");
            entity.Property(e => e.HtQuyenId)
                .HasMaxLength(50)
                .HasColumnName("HT_QUYEN_ID");

            entity.HasOne(d => d.HtNhomQuyen).WithMany(p => p.HtQuyenNhomQuyens)
                .HasForeignKey(d => d.HtNhomQuyenId)
                .HasConstraintName("FK_HT_QUYEN_NHOM_QUYEN_HT_NHOM_QUYEN");

            entity.HasOne(d => d.HtQuyen).WithMany(p => p.HtQuyenNhomQuyens)
                .HasForeignKey(d => d.HtQuyenId)
                .HasConstraintName("FK_HT_QUYEN_NHOM_QUYEN_HT_QUYEN");
        });

        modelBuilder.Entity<HtThamSoHeThong>(entity =>
        {
            entity.ToTable("HT_THAM_SO_HE_THONG");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.ApDungMapExcel)
                .HasMaxLength(250)
                .HasColumnName("AP_DUNG_MAP_EXCEL");
            entity.Property(e => e.DinhKyTuan)
                .HasMaxLength(250)
                .HasColumnName("DINH_KY_TUAN");
            entity.Property(e => e.ImagePath)
                .HasMaxLength(250)
                .HasColumnName("IMAGE_PATH");
            entity.Property(e => e.SmtpServer)
                .HasMaxLength(250)
                .HasColumnName("SMTP_SERVER");
            entity.Property(e => e.TypeDocument)
                .HasMaxLength(250)
                .HasColumnName("TYPE_DOCUMENT");
        });

        modelBuilder.Entity<QuanLyThuTucBaoCaoTongHopVeGoiThau>(entity =>
        {
            entity.ToTable("QUAN_LY_THU_TUC_BAO_CAO_TONG_HOP_VE_GOI_THAU");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.ChenhLechDoQuocHoiChuTruongDauTu).HasColumnName("CHENH_LECH_DO_QUOC_HOI_CHU_TRUONG_DAU_TU");
            entity.Property(e => e.ChenhLechDuAnNhomA).HasColumnName("CHENH_LECH_DU_AN_NHOM_A");
            entity.Property(e => e.ChenhLechDuAnNhomB).HasColumnName("CHENH_LECH_DU_AN_NHOM_B");
            entity.Property(e => e.ChenhLechDuAnNhomC).HasColumnName("CHENH_LECH_DU_AN_NHOM_C");
            entity.Property(e => e.ChenhLechTongCong).HasColumnName("CHENH_LECH_TONG_CONG");
            entity.Property(e => e.HinhThucDauThau).HasColumnName("HINH_THUC_DAU_THAU");
            entity.Property(e => e.IdBaoCao)
                .HasMaxLength(50)
                .HasColumnName("ID_BAO_CAO");
            entity.Property(e => e.LinhVucVaHinhThuc).HasColumnName("LINH_VUC_VA_HINH_THUC");
            entity.Property(e => e.NguonVon).HasColumnName("NGUON_VON");
            entity.Property(e => e.TongGiaGoiThauDoQuocHoiChuTruongDauTu).HasColumnName("TONG_GIA_GOI_THAU_DO_QUOC_HOI_CHU_TRUONG_DAU_TU");
            entity.Property(e => e.TongGiaGoiThauDuAnNhomA).HasColumnName("TONG_GIA_GOI_THAU_DU_AN_NHOM_A");
            entity.Property(e => e.TongGiaGoiThauDuAnNhomB).HasColumnName("TONG_GIA_GOI_THAU_DU_AN_NHOM_B");
            entity.Property(e => e.TongGiaGoiThauDuAnNhomC).HasColumnName("TONG_GIA_GOI_THAU_DU_AN_NHOM_C");
            entity.Property(e => e.TongGiaGoiThauTongCong).HasColumnName("TONG_GIA_GOI_THAU_TONG_CONG");
            entity.Property(e => e.TongGiaTrungThauDoQuocHoiChuTruongDauTu).HasColumnName("TONG_GIA_TRUNG_THAU_DO_QUOC_HOI_CHU_TRUONG_DAU_TU");
            entity.Property(e => e.TongGiaTrungThauDuAnNhomA).HasColumnName("TONG_GIA_TRUNG_THAU_DU_AN_NHOM_A");
            entity.Property(e => e.TongGiaTrungThauDuAnNhomB).HasColumnName("TONG_GIA_TRUNG_THAU_DU_AN_NHOM_B");
            entity.Property(e => e.TongGiaTrungThauDuAnNhomC).HasColumnName("TONG_GIA_TRUNG_THAU_DU_AN_NHOM_C");
            entity.Property(e => e.TongGiaTrungThauTongCong).HasColumnName("TONG_GIA_TRUNG_THAU_TONG_CONG");
            entity.Property(e => e.TongSoGoiThauDoQuocHoiChuTruongDauTu).HasColumnName("TONG_SO_GOI_THAU_DO_QUOC_HOI_CHU_TRUONG_DAU_TU");
            entity.Property(e => e.TongSoGoiThauDuAnNhomA).HasColumnName("TONG_SO_GOI_THAU_DU_AN_NHOM_A");
            entity.Property(e => e.TongSoGoiThauDuAnNhomB).HasColumnName("TONG_SO_GOI_THAU_DU_AN_NHOM_B");
            entity.Property(e => e.TongSoGoiThauDuAnNhomC).HasColumnName("TONG_SO_GOI_THAU_DU_AN_NHOM_C");
            entity.Property(e => e.TongSoGoiThauTongCong).HasColumnName("TONG_SO_GOI_THAU_TONG_CONG");

            entity.HasOne(d => d.IdBaoCaoNavigation).WithMany(p => p.QuanLyThuTucBaoCaoTongHopVeGoiThaus)
                .HasForeignKey(d => d.IdBaoCao)
                .HasConstraintName("FK_QUAN_LY_THU_TUC_BAO_CAO_TONG_HOP_VE_GOI_THAU_QUAN_LY_THU_TUC_BAO_CAO_TONG_HOP_VE_GOI_THAU");
        });

        modelBuilder.Entity<QuanLyThuTucCnkq>(entity =>
        {
            entity.ToTable("QUAN_LY_THU_TUC_CNKQ");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.ChuDauTu)
                .HasMaxLength(50)
                .HasColumnName("CHU_DAU_TU");
            entity.Property(e => e.CoCauNguonVon)
                .HasMaxLength(50)
                .HasColumnName("CO_CAU_NGUON_VON");
            entity.Property(e => e.CoCauNguonVonDc)
                .HasMaxLength(50)
                .HasColumnName("CO_CAU_NGUON_VON_DC");
            entity.Property(e => e.CpBoithuong).HasColumnName("CP_BOITHUONG");
            entity.Property(e => e.CpChung).HasColumnName("CP_CHUNG");
            entity.Property(e => e.CpDuphong).HasColumnName("CP_DUPHONG");
            entity.Property(e => e.CpThietbi).HasColumnName("CP_THIETBI");
            entity.Property(e => e.CpXayDung).HasColumnName("CP_XAY_DUNG");
            entity.Property(e => e.DiaDiemDauTu)
                .HasMaxLength(500)
                .HasColumnName("DIA_DIEM_DAU_TU");
            entity.Property(e => e.DuAnId)
                .HasMaxLength(50)
                .HasColumnName("DU_AN_ID");
            entity.Property(e => e.DutoanCpCbdautu).HasColumnName("DUTOAN_CP_CBDAUTU");
            entity.Property(e => e.DutoanCpCbdautuDc).HasColumnName("DUTOAN_CP_CBDAUTU_DC");
            entity.Property(e => e.GhiChu)
                .HasMaxLength(500)
                .HasColumnName("GHI_CHU");
            entity.Property(e => e.HinhThucQuanly)
                .HasMaxLength(500)
                .HasColumnName("HINH_THUC_QUANLY");
            entity.Property(e => e.IdThuTuc)
                .HasMaxLength(50)
                .HasColumnName("ID_THU_TUC");
            entity.Property(e => e.NamPheDuyet)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("NAM_PHE_DUYET");
            entity.Property(e => e.NangLucThietKe)
                .HasMaxLength(250)
                .HasColumnName("NANG_LUC_THIET_KE");
            entity.Property(e => e.NhomDuAn).HasColumnName("NHOM_DU_AN");
            entity.Property(e => e.SoBuocThietKe)
                .HasMaxLength(500)
                .HasColumnName("SO_BUOC_THIET_KE");
            entity.Property(e => e.SoNgayQuyetDinh)
                .HasMaxLength(50)
                .HasColumnName("SO_NGAY_QUYET_DINH");
            entity.Property(e => e.SoNgayQuyetDinhBiDc)
                .HasMaxLength(50)
                .HasColumnName("SO_NGAY_QUYET_DINH_BI_DC");
            entity.Property(e => e.TenKeHoach)
                .HasMaxLength(500)
                .HasColumnName("TEN_KE_HOACH");
            entity.Property(e => e.ThoigianThuchien)
                .HasMaxLength(500)
                .HasColumnName("THOIGIAN_THUCHIEN");
            entity.Property(e => e.ThoigianThuchienDc)
                .HasMaxLength(500)
                .HasColumnName("THOIGIAN_THUCHIEN_DC");
            entity.Property(e => e.TiendoThuchien)
                .HasMaxLength(500)
                .HasColumnName("TIENDO_THUCHIEN");
            entity.Property(e => e.TongMucDauTu).HasColumnName("TONG_MUC_DAU_TU");

            entity.HasOne(d => d.DuAn).WithMany(p => p.QuanLyThuTucCnkqs)
                .HasForeignKey(d => d.DuAnId)
                .HasConstraintName("FK_QUAN_LY_THU_TUC_CNKQ_QUAN_LY_THU_TUC_CNKQ1");

            entity.HasOne(d => d.IdThuTucNavigation).WithMany(p => p.QuanLyThuTucCnkqs)
                .HasForeignKey(d => d.IdThuTuc)
                .HasConstraintName("FK_QUAN_LY_THU_TUC_CNKQ_QUAN_LY_THU_TUC_CNKQ");
        });

        modelBuilder.Entity<QuanLyThuTucFile>(entity =>
        {
            entity.ToTable("QUAN_LY_THU_TUC_FILE");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.FileDinhKem)
                .HasMaxLength(250)
                .HasColumnName("FILE_DINH_KEM");
            entity.Property(e => e.FilePath)
                .HasMaxLength(250)
                .HasColumnName("FILE_PATH");
            entity.Property(e => e.IdThuTuc)
                .HasMaxLength(50)
                .HasColumnName("ID_THU_TUC");
            entity.Property(e => e.Loai).HasColumnName("LOAI");
        });

        modelBuilder.Entity<QuanLyThuTucNoiBoBaoCaoTongHop>(entity =>
        {
            entity.ToTable("QUAN_LY_THU_TUC_NOI_BO_BAO_CAO_TONG_HOP");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.LoaiBaoCaoTongHop)
                .HasComment("1: Báo cáo tổng hợp về thủ tục nội bộ - 2: báo cáo tổng hợp về gói thầu ")
                .HasColumnName("LOAI_BAO_CAO_TONG_HOP");
            entity.Property(e => e.Nam)
                .HasMaxLength(50)
                .HasColumnName("NAM");
            entity.Property(e => e.NgayTao)
                .HasColumnType("datetime")
                .HasColumnName("NGAY_TAO");
            entity.Property(e => e.NguoiTao)
                .HasMaxLength(50)
                .HasColumnName("NGUOI_TAO");
            entity.Property(e => e.Ten)
                .HasMaxLength(100)
                .HasColumnName("TEN");
        });

        modelBuilder.Entity<QuanLyThuTucNoiBoBaoCaoTongHopChiTiet>(entity =>
        {
            entity.ToTable("QUAN_LY_THU_TUC_NOI_BO_BAO_CAO_TONG_HOP_CHI_TIET");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.GhiChu)
                .HasMaxLength(250)
                .HasColumnName("GHI_CHU");
            entity.Property(e => e.IdBaoCao)
                .HasMaxLength(50)
                .HasColumnName("ID_BAO_CAO");
            entity.Property(e => e.LoaiHoSo).HasColumnName("LOAI_HO_SO");
            entity.Property(e => e.SlHsChuaLuuKho).HasColumnName("SL_HS_CHUA_LUU_KHO");
            entity.Property(e => e.SlHsDaGqDungHan).HasColumnName("SL_HS_DA_GQ_DUNG_HAN");
            entity.Property(e => e.SlHsDaGqQuaHan).HasColumnName("SL_HS_DA_GQ_QUA_HAN");
            entity.Property(e => e.SlHsDaGqTruocHan).HasColumnName("SL_HS_DA_GQ_TRUOC_HAN");
            entity.Property(e => e.SlHsDangGqQuaHan).HasColumnName("SL_HS_DANG_GQ_QUA_HAN");
            entity.Property(e => e.SlHsDangGqTrongHan).HasColumnName("SL_HS_DANG_GQ_TRONG_HAN");
            entity.Property(e => e.SlHsLuuKho).HasColumnName("SL_HS_LUU_KHO");
            entity.Property(e => e.TongSoHsDaGiaiQuyet).HasColumnName("TONG_SO_HS_DA_GIAI_QUYET");
            entity.Property(e => e.TongSoHsDaHoanThanh).HasColumnName("TONG_SO_HS_DA_HOAN_THANH");
            entity.Property(e => e.TongSoHsDangGiaiQuyet).HasColumnName("TONG_SO_HS_DANG_GIAI_QUYET");
            entity.Property(e => e.TongSoHsTiepNhan).HasColumnName("TONG_SO_HS_TIEP_NHAN");

            entity.HasOne(d => d.IdBaoCaoNavigation).WithMany(p => p.QuanLyThuTucNoiBoBaoCaoTongHopChiTiets)
                .HasForeignKey(d => d.IdBaoCao)
                .HasConstraintName("FK_QUAN_LY_THU_TUC_NOI_BO_BAO_CAO_TONG_HOP_CHI_TIET_QUAN_LY_THU_TUC_NOI_BO_BAO_CAO_TONG_HOP_CHI_TIET");
        });

        modelBuilder.Entity<QuanLyThuTucNoiBoChuyenThuLy>(entity =>
        {
            entity.ToTable("QUAN_LY_THU_TUC_NOI_BO_CHUYEN_THU_LY");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.ChuyenVien)
                .HasMaxLength(50)
                .HasColumnName("CHUYEN_VIEN");
            entity.Property(e => e.IdThuTuc)
                .HasMaxLength(50)
                .HasColumnName("ID_THU_TUC");
            entity.Property(e => e.NgayChuyenThuLy)
                .HasColumnType("datetime")
                .HasColumnName("NGAY_CHUYEN_THU_LY");
            entity.Property(e => e.PhongBan)
                .HasMaxLength(50)
                .HasColumnName("PHONG_BAN");

            entity.HasOne(d => d.IdThuTucNavigation).WithMany(p => p.QuanLyThuTucNoiBoChuyenThuLies)
                .HasForeignKey(d => d.IdThuTuc)
                .HasConstraintName("FK_QUAN_LY_THU_TUC_NOI_BO_CHUYEN_THU_LY_QUAN_LY_THU_TUC_NOI_BO_CHUYEN_THU_LY");
        });

        modelBuilder.Entity<QuanLyThuTucNoiBoDuAnDtc>(entity =>
        {
            entity.ToTable("QUAN_LY_THU_TUC_NOI_BO_DU_AN_DTC");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.CacThongTinKhac)
                .HasMaxLength(250)
                .HasColumnName("CAC_THONG_TIN_KHAC");
            entity.Property(e => e.DuKienHoanThanh)
                .HasColumnType("datetime")
                .HasColumnName("DU_KIEN_HOAN_THANH");
            entity.Property(e => e.IdDonViThucHienDuAn)
                .HasMaxLength(50)
                .HasColumnName("ID_DON_VI_THUC_HIEN_DU_AN");
            entity.Property(e => e.IsXoa)
                .HasComment("1:  Đã xóa")
                .HasColumnName("IS_XOA");
            entity.Property(e => e.LoaiHoSo)
                .HasComment("1 - Thủ tục thẩm định dự toán chuẩn bị đầu tư/ \r\n2 - Thủ tục thẩm định chủ trương đầu tư/ \r\n3 - thẩm định điều chỉnh chủ trương đầu tư/ \r\n4 - thủ tục thẩm đỉnh dự án đầu tư có cấu phần xây dưng/ \r\n5 - thủ tục thẩm định điều chỉnh dự án đầu tư có cấu phần xây dựng/ \r\n6 - thủ tục thẩm định dự dự án đầu tư không có cấu phần xây dựng/ \r\n7 - thủ tục thẩm định điều chỉnh dự án đầu tư không có cấu phần xây dựng/ \r\n8 - thủ tục thẩm định dự toán dự án đầu tư không có cấu phần xây dựng/ \r\n9 - thủ tục thẩm định dự toán điều chỉnh dự án đầu tư không có cấu phần xây dựng/ ")
                .HasColumnName("LOAI_HO_SO");
            entity.Property(e => e.LuuKho).HasColumnName("LUU_KHO");
            entity.Property(e => e.MaHoSo)
                .HasMaxLength(50)
                .HasColumnName("MA_HO_SO");
            entity.Property(e => e.NgayNhanHoSo)
                .HasColumnType("datetime")
                .HasColumnName("NGAY_NHAN_HO_SO");
            entity.Property(e => e.NgayTao)
                .HasColumnType("datetime")
                .HasColumnName("NGAY_TAO");
            entity.Property(e => e.NguoiTao)
                .HasMaxLength(50)
                .HasColumnName("NGUOI_TAO");
            entity.Property(e => e.NhomDuAn).HasColumnName("NHOM_DU_AN");
            entity.Property(e => e.TenHoSo)
                .HasMaxLength(250)
                .HasColumnName("TEN_HO_SO");
            entity.Property(e => e.VanBanChuDauTu)
                .HasMaxLength(250)
                .HasColumnName("VAN_BAN_CHU_DAU_TU");
        });

        modelBuilder.Entity<QuanLyThuTucNoiBoDuAnDtcKqth>(entity =>
        {
            entity.ToTable("QUAN_LY_THU_TUC_NOI_BO_DU_AN_DTC_KQTH");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.IdThuTuc)
                .HasMaxLength(50)
                .HasColumnName("ID_THU_TUC");
            entity.Property(e => e.NgayKy1)
                .HasColumnType("datetime")
                .HasColumnName("NGAY_KY1");
            entity.Property(e => e.NgayKy2)
                .HasColumnType("datetime")
                .HasColumnName("NGAY_KY2");
            entity.Property(e => e.SoNgayVb1)
                .HasMaxLength(50)
                .HasColumnName("SO_NGAY_VB1");
            entity.Property(e => e.SoNgayVb2)
                .HasMaxLength(50)
                .HasColumnName("SO_NGAY_VB2");

            entity.HasOne(d => d.IdThuTucNavigation).WithMany(p => p.QuanLyThuTucNoiBoDuAnDtcKqths)
                .HasForeignKey(d => d.IdThuTuc)
                .HasConstraintName("FK_QUAN_LY_THU_TUC_NOI_BO_DU_AN_DTC_KQTH_QUAN_LY_THU_TUC_NOI_BO_DU_AN_DTC");
        });

        modelBuilder.Entity<QuanLyThuTucNoiBoDuAnDtcNguonVon>(entity =>
        {
            entity.ToTable("QUAN_LY_THU_TUC_NOI_BO_DU_AN_DTC_NGUON_VON");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.GiaTriNguonVon).HasColumnName("GIA_TRI_NGUON_VON");
            entity.Property(e => e.IdThuTuc)
                .HasMaxLength(50)
                .HasColumnName("ID_THU_TUC");
            entity.Property(e => e.LoaiNguonVon)
                .HasMaxLength(50)
                .HasColumnName("LOAI_NGUON_VON");

            entity.HasOne(d => d.IdThuTucNavigation).WithMany(p => p.QuanLyThuTucNoiBoDuAnDtcNguonVons)
                .HasForeignKey(d => d.IdThuTuc)
                .HasConstraintName("FK_QUAN_LY_THU_TUC_NOI_BO_DU_AN_DTC_NGUON_VON_QUAN_LY_THU_TUC_NOI_BO_DU_AN_DTC");
        });

        modelBuilder.Entity<QuanLyThuTucNoiBoDuAnDtcPhieuXuLy>(entity =>
        {
            entity.ToTable("QUAN_LY_THU_TUC_NOI_BO_DU_AN_DTC_PHIEU_XU_LY");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.Buoc1).HasColumnName("BUOC_1");
            entity.Property(e => e.Buoc2).HasColumnName("BUOC_2");
            entity.Property(e => e.Buoc3).HasColumnName("BUOC_3");
            entity.Property(e => e.Buoc4).HasColumnName("BUOC_4");
            entity.Property(e => e.Buoc5).HasColumnName("BUOC_5");
            entity.Property(e => e.Buoc6).HasColumnName("BUOC_6");
            entity.Property(e => e.IdThuTuc)
                .HasMaxLength(50)
                .HasColumnName("ID_THU_TUC");
            entity.Property(e => e.TextBuoc1)
                .HasMaxLength(250)
                .HasColumnName("TEXT_BUOC_1");
            entity.Property(e => e.TextBuoc2)
                .HasMaxLength(250)
                .HasColumnName("TEXT_BUOC_2");
            entity.Property(e => e.TextBuoc3)
                .HasMaxLength(250)
                .HasColumnName("TEXT_BUOC_3");
            entity.Property(e => e.TextBuoc4)
                .HasMaxLength(250)
                .HasColumnName("TEXT_BUOC_4");
            entity.Property(e => e.TextBuoc5)
                .HasMaxLength(250)
                .HasColumnName("TEXT_BUOC_5");
            entity.Property(e => e.TextBuoc6)
                .HasMaxLength(250)
                .HasColumnName("TEXT_BUOC_6");
            entity.Property(e => e.ThoiGianThucHien).HasColumnName("THOI_GIAN_THUC_HIEN");

            entity.HasOne(d => d.IdThuTucNavigation).WithMany(p => p.QuanLyThuTucNoiBoDuAnDtcPhieuXuLies)
                .HasForeignKey(d => d.IdThuTuc)
                .HasConstraintName("FK_QUAN_LY_THU_TUC_NOI_BO_DU_AN_DTC_PHIEU_XU_LY_QUAN_LY_THU_TUC_NOI_BO_DU_AN_DTC");
        });

        modelBuilder.Entity<QuanLyThuTucNoiBoDuAnDtcTienDoXuLy>(entity =>
        {
            entity.ToTable("QUAN_LY_THU_TUC_NOI_BO_DU_AN_DTC_TIEN_DO_XU_LY");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.GhiChuTinhTrang)
                .HasMaxLength(250)
                .HasColumnName("GHI_CHU_TINH_TRANG");
            entity.Property(e => e.IdChuyenVienThuLy)
                .HasMaxLength(50)
                .HasColumnName("ID_CHUYEN_VIEN_THU_LY");
            entity.Property(e => e.IdThuTuc)
                .HasMaxLength(50)
                .HasColumnName("ID_THU_TUC");
            entity.Property(e => e.NgayGiaiQuyet)
                .HasColumnType("datetime")
                .HasColumnName("NGAY_GIAI_QUYET");
            entity.Property(e => e.SoNgayQuyetDinh)
                .HasMaxLength(250)
                .HasColumnName("SO_NGAY_QUYET_DINH");
            entity.Property(e => e.TrangThai).HasColumnName("TRANG_THAI");

            entity.HasOne(d => d.IdChuyenVienThuLyNavigation).WithMany(p => p.QuanLyThuTucNoiBoDuAnDtcTienDoXuLies)
                .HasForeignKey(d => d.IdChuyenVienThuLy)
                .HasConstraintName("FK_QUAN_LY_THU_TUC_NOI_BO_DU_AN_DTC_TIEN_DO_XU_LY_HT_NGUOI_DUNG");

            entity.HasOne(d => d.IdThuTucNavigation).WithMany(p => p.QuanLyThuTucNoiBoDuAnDtcTienDoXuLies)
                .HasForeignKey(d => d.IdThuTuc)
                .HasConstraintName("FK_QUAN_LY_THU_TUC_NOI_BO_DU_AN_DTC_TIEN_DO_XU_LY_QUAN_LY_THU_TUC_NOI_BO_DU_AN_DTC");
        });

        modelBuilder.Entity<ThuTucNbLuaChonNhaThauPhanChiaGoiThau>(entity =>
        {
            entity.ToTable("THU_TUC_NB_LUA_CHON_NHA_THAU_PHAN_CHIA_GOI_THAU");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.GiaGoiThau).HasColumnName("GIA_GOI_THAU");
            entity.Property(e => e.GiaTrungThau).HasColumnName("GIA_TRUNG_THAU");
            entity.Property(e => e.HinhThucDauThau).HasColumnName("HINH_THUC_DAU_THAU");
            entity.Property(e => e.HinhThucLuaChonNhaThau).HasColumnName("HINH_THUC_LUA_CHON_NHA_THAU");
            entity.Property(e => e.IdThuTuc)
                .HasMaxLength(50)
                .HasColumnName("ID_THU_TUC");
            entity.Property(e => e.LinhVuc).HasColumnName("LINH_VUC");
            entity.Property(e => e.NguonVon).HasColumnName("NGUON_VON");
            entity.Property(e => e.TenGoiThau)
                .HasMaxLength(250)
                .HasColumnName("TEN_GOI_THAU");

            entity.HasOne(d => d.IdThuTucNavigation).WithMany(p => p.ThuTucNbLuaChonNhaThauPhanChiaGoiThaus)
                .HasForeignKey(d => d.IdThuTuc)
                .HasConstraintName("FK_THU_TUC_NB_LUA_CHON_NHA_THAU_PHAN_CHIA_GOI_THAU_QUAN_LY_THU_TUC_NOI_BO_DU_AN_DTC");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
